namespace Oliviann.Tests.ComponentModel
{
    #region Usings

    using System;
    using Oliviann.ComponentModel;
    using TestObjects;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class NotifyPropertyChangeTests
    {
        #region OnPropertyChange Tests

        [Fact]
        public void NotifyPropertyChangeTest_Nonsubscribe()
        {
            bool eventFired = false;
            var obj = new MocPropChangeClass();

            obj.PropInt = 10;
            obj.PropString = "Test";

            Assert.Equal(10, obj.PropInt);
            Assert.Equal("Test", obj.PropString);
            Assert.False(eventFired);
        }

        [Fact]
        public void NotifyPropertyChangeTest_Subscribe()
        {
            bool eventFired = false;
            string property = string.Empty;
            object oldValue = null;
            object newValue = null;

            var obj = new MocPropChangeClass();
            obj.PropertyChange += (sender, args) =>
                {
                    eventFired = true;
                    property = args.PropertyName;
                    oldValue = args.OldValue;
                    newValue = args.NewValue;
                };
            obj.PropInt = 13;

            Assert.Equal(13, obj.PropInt);
            Assert.True(eventFired);
            Assert.Equal("PropInt", property);
            Assert.Equal(0, (int)oldValue);
            Assert.Equal(13, (int)newValue);

            eventFired = false;
            property = string.Empty;
            oldValue = null;
            newValue = null;

            obj.PropString = "Test";

            Assert.Equal("Test", obj.PropString);
            Assert.True(eventFired);
            Assert.Equal("PropString", property);
            Assert.Null((string)oldValue);
            Assert.Equal("Test", (string)newValue);
        }

        [Fact]
        public void NotifyPropertyChangeTest_SubscribeAndClear()
        {
            bool eventFired = false;
            string property = string.Empty;
            object oldValue = null;
            object newValue = null;

            var obj = new MocPropChangeClass();
            obj.PropertyChange += (sender, args) =>
                {
                    eventFired = true;
                    property = args.PropertyName;
                    oldValue = args.OldValue;
                    newValue = args.NewValue;
                };
            obj.PropInt = 13;

            Assert.Equal(13, obj.PropInt);
            Assert.True(eventFired);
            Assert.Equal("PropInt", property);
            Assert.Equal(0, (int)oldValue);
            Assert.Equal(13, (int)newValue);

            obj.ClearEvents();

            eventFired = false;
            property = string.Empty;
            oldValue = null;
            newValue = null;

            obj.PropString = "Test";

            Assert.Equal("Test", obj.PropString);
            Assert.False(eventFired);
            Assert.Equal(string.Empty, property);
            Assert.Null((string)oldValue);
            Assert.Null((string)newValue);
        }

        [Fact]
        public void NotifyPropertyChangeTest_SubscribeAndUnsubscribe()
        {
            bool eventFired = false;
            string property = string.Empty;
            object oldValue = null;
            object newValue = null;

            EventHandler<PropertyChangeEventArgs> handler = (sender, args) =>
                {
                    eventFired = true;
                    property = args.PropertyName;
                    oldValue = args.OldValue;
                    newValue = args.NewValue;
                };

            var obj = new MocPropChangeClass();
            obj.PropertyChange += handler;
            obj.PropInt = 13;

            Assert.Equal(13, obj.PropInt);
            Assert.True(eventFired);
            Assert.Equal("PropInt", property);
            Assert.Equal(0, (int)oldValue);
            Assert.Equal(13, (int)newValue);

            obj.PropertyChange -= handler;

            eventFired = false;
            property = string.Empty;
            oldValue = null;
            newValue = null;

            obj.PropString = "Test";

            Assert.Equal("Test", obj.PropString);
            Assert.False(eventFired);
            Assert.Equal(string.Empty, property);
            Assert.Null((string)oldValue);
            Assert.Null((string)newValue);
        }

        [Fact]
        public void NotifyPropertyChangeTest_Subscribe2x()
        {
            bool eventFired = false;
            string property = string.Empty;
            object oldValue = null;
            object newValue = null;

            EventHandler<PropertyChangeEventArgs> handler = (sender, args) =>
                {
                    eventFired = true;
                    property = args.PropertyName;
                    oldValue = args.OldValue;
                    newValue = args.NewValue;
                };

            var obj = new MocPropChangeClass();
            obj.PropertyChange += handler;
            obj.PropertyChange += handler;
            obj.PropInt = 13;

            Assert.Equal(13, obj.PropInt);
            Assert.True(eventFired);
            Assert.Equal("PropInt", property);
            Assert.Equal(0, (int)oldValue);
            Assert.Equal(13, (int)newValue);
        }

        #endregion OnPropertyChange Tests

        #region SetProperty Tests

        /// <summary>
        /// Verifies setting a property to the same value doesn't fire the
        /// event.
        /// </summary>
        [Fact]
        public void SetPropertyTest_SameValueNotFire()
        {
            bool eventFired = false;

            var obj = new MocPropChangeClass { PropString2 = "Oliviann" };
            obj.PropertyChange += (sender, args) => { eventFired = true; };

            obj.PropString2 = "Oliviann";

            Assert.False(eventFired);
        }

        /// <summary>
        /// Verifies a new value is set to the property and the event is fired.
        /// </summary>
        [Fact]
        public void SetPropertyTest_DifferentValue()
        {
            bool eventFired = false;
            string property = string.Empty;
            object oldValue = null;
            object newValue = null;

            var obj = new MocPropChangeClass { PropLong = 99L };
            obj.PropertyChange += (sender, args) =>
                {
                    eventFired = true;
                    property = args.PropertyName;
                    oldValue = args.OldValue;
                    newValue = args.NewValue;
                };

            obj.PropLong = 21L;

            Assert.Equal(21L, obj.PropLong);
            Assert.True(eventFired);
            Assert.Equal("PropLong", property);
            Assert.Equal(99L, (long)oldValue);
            Assert.Equal(21L, (long)newValue);
        }

        #endregion SetProperty Tests
    }
}