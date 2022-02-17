namespace Oliviann.Tests.ComponentModel.Composition
{
    #region Usings

    using Oliviann.ComponentModel.Composition;
    using Oliviann.Tests.TestObjects;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ExportServiceMetadataAttributeTests
    {
        [Fact]
        public void ExportServiceMetadataTest_ctor_Default()
        {
            var att = new ExportServiceMetadataAttribute();
            string ctName = att.ContractName;
            string htName = att.HostName;
            uint order = att.LoadOrder;

            Assert.Null(ctName);
            Assert.Null(htName);
            Assert.Equal(uint.MaxValue, order);
        }

        [Fact]
        public void ExportServiceMetadataTest_ctor_NullName()
        {
            var att = new ExportServiceMetadataAttribute(contractName: null);
            string ctName = att.ContractName;
            string htName = att.HostName;
            uint order = att.LoadOrder;

            Assert.Null(ctName);
            Assert.Null(htName);
            Assert.Equal(uint.MaxValue, order);
        }

        [Fact]
        public void ExportServiceMetadataTest_ctor_StringName()
        {
            var att = new ExportServiceMetadataAttribute("BART");
            string ctName = att.ContractName;
            string htName = att.HostName;
            uint order = att.LoadOrder;

            Assert.Equal("BART", ctName);
            Assert.Null(htName);
            Assert.Equal(uint.MaxValue, order);
        }

        [Fact]
        public void ExportServiceMetadataTest_ctor_NullContract()
        {
            var att = new ExportServiceMetadataAttribute(contractType: null);
            string ctName = att.ContractName;
            string htName = att.HostName;
            uint order = att.LoadOrder;

            Assert.Null(ctName);
            Assert.Null(htName);
            Assert.Equal(uint.MaxValue, order);
        }

        [Fact]
        public void ExportServiceMetadataTest_ctor_TypeContract()
        {
            var att = new ExportServiceMetadataAttribute(typeof(GenericMocTestClass));
            string ctName = att.ContractName;
            string htName = att.HostName;
            uint order = att.LoadOrder;

            Assert.Null(ctName);
            Assert.Null(htName);
            Assert.Equal(uint.MaxValue, order);
        }

        [Fact]
        public void ExportServiceMetadataTest_ctor_NullContractNullType()
        {
            var att = new ExportServiceMetadataAttribute(null, null);
            string ctName = att.ContractName;
            string htName = att.HostName;
            uint order = att.LoadOrder;

            Assert.Null(ctName);
            Assert.Null(htName);
            Assert.Equal(uint.MaxValue, order);
        }

        [Fact]
        public void ExportServiceMetadataTest_ctor_StringContractNullType()
        {
            var att = new ExportServiceMetadataAttribute("HOMER", null);
            string ctName = att.ContractName;
            string htName = att.HostName;
            uint order = att.LoadOrder;

            Assert.Equal("HOMER", ctName);
            Assert.Null(htName);
            Assert.Equal(uint.MaxValue, order);
        }

        [Fact]
        public void ExportServiceMetadataTest_ctor_StringContractValueType()
        {
            var att = new ExportServiceMetadataAttribute("MARGE", typeof(GenericMocTestClass));
            string ctName = att.ContractName;
            string htName = att.HostName;
            uint order = att.LoadOrder;

            Assert.Equal("MARGE", ctName);
            Assert.Null(htName);
            Assert.Equal(uint.MaxValue, order);
        }

        [Fact]
        public void ExportServiceMetadataTest_StringHostname()
        {
            var att = new ExportServiceMetadataAttribute("LISA", typeof(GenericMocTestClass)) { HostName = "Oliviann.Something" };
            string ctName = att.ContractName;
            string htName = att.HostName;
            uint order = att.LoadOrder;

            Assert.Equal("LISA", ctName);
            Assert.Equal("Oliviann.Something", htName);
            Assert.Equal(uint.MaxValue, order);
        }

        [Fact]
        public void ExportServiceMetadataTest_SetOrder()
        {
            var att = new ExportServiceMetadataAttribute("BOB", typeof(GenericMocTestClass))
                {
                    HostName = "Oliviann.Something",
                    LoadOrder = 100
                };
            string ctName = att.ContractName;
            string htName = att.HostName;
            uint order = att.LoadOrder;

            Assert.Equal("BOB", ctName);
            Assert.Equal("Oliviann.Something", htName);
            Assert.Equal(100U, order);
        }
    }
}