namespace Oliviann.Tests.Collections.Specialized
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Oliviann.Collections.Specialized;
    using Xunit;
    using Xunit.Abstractions;

    #endregion Usings

    [Trait("Category", "NOT_FINISHED")]
    public class ObservableJobQueueTests
    {
        #region Fields

        private object something = new object();
        private readonly ITestOutputHelper output;

        #endregion Fields

        public ObservableJobQueueTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void QueueJobTest_NullJob()
        {
            var que = new ObservableJobQueue<string>();
            Assert.Throws<ArgumentNullException>(() => que.Enqueue(null));
        }

        [Fact]
        public void QueueJobTest_ValidJob()
        {
            var que = new ObservableJobQueue<string>(new JobQueueOptions { MaxConcurrentJobs = 1 });

            int workerActionCount = 0;
            que.WorkerAction += j =>
            {
                workerActionCount += 1;
                Thread.Sleep(500);
            };

            var job = new Job<string>("Test Job");
            que.Enqueue(job);
        }

        [Fact]
        public void QueueJobTest_50Jobs_1Concurrent()
        {
            var que = new ObservableJobQueue<string>(new JobQueueOptions { MaxConcurrentJobs = 1 });
            var source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Task mon = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    this.output.WriteLine($"Wait: {que.WaitingCount} Work: {que.WorkingCount}");
                    Thread.Sleep(2);
                }
            }, token);

            int workerActionCount = 0;
            que.WorkerAction += j =>
            {
                Interlocked.Increment(ref workerActionCount);
                Thread.Sleep(new Random().Next(400, 1000));
            };

            Parallel.ForEach(Enumerable.Range(1, 50), count =>
            {
                var job = new Job<string>("Test Job:" + count);
                que.Enqueue(job);
            });

            while (que.Count > 0)
            {
                Assert.InRange(que.WorkingCount, 0, 1);
                Thread.Sleep(5);
            }

            source.Cancel(false);
            Assert.Equal(50, workerActionCount);
        }

        [Fact]
        public void QueueJobTest_100Jobs_4Concurrent()
        {
            var que = new ObservableJobQueue<string>(new JobQueueOptions { MaxConcurrentJobs = 4 });
            que.Logger += (sender, args) => this.output.WriteLine(args.Value2);

            var source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Task mon = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    this.output.WriteLine($"Wait: {que.WaitingCount} Work: {que.WorkingCount}");
                    Thread.Sleep(2);
                }
            }, token);

            var rand = new Random();
            int workerActionCount = 0;
            que.WorkerAction += j =>
            {
                Interlocked.Increment(ref workerActionCount);
                Thread.Sleep(rand.Next(400, 1000));
            };

            Parallel.ForEach(Enumerable.Range(1, 100), count =>
            {
                var job = new Job<string>("Test Job:" + count);
                que.Enqueue(job);
            });

            while (que.Count > 0)
            {
                Assert.InRange(que.WorkingCount, 0, 4);
                Thread.Sleep(5);
            }

            source.Cancel(false);
            Assert.Equal(100, workerActionCount);
        }
    }
}