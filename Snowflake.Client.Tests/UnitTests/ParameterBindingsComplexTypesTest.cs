using Xunit;

namespace Snowflake.Client.Tests.UnitTests
{

    public class ParameterBindingsComplexTypesTest
    {
        [Fact]
        public void BuildParameters_FromClass_Properties()
        {
            var value = new CustomClassWithProperties() { IntProperty = 2, StringProperty = "test" };

            var bindings = ParameterBinder.BuildParameterBindings(value);

            Assert.True(bindings.ContainsKey(nameof(value.IntProperty)));
            Assert.True(bindings[nameof(value.IntProperty)].Type == "FIXED");
            Assert.True(bindings[nameof(value.IntProperty)].Value == value.IntProperty.ToString());

            Assert.True(bindings.ContainsKey(nameof(value.StringProperty)));
            Assert.True(bindings[nameof(value.StringProperty)].Type == "TEXT");
            Assert.True(bindings[nameof(value.StringProperty)].Value == value.StringProperty);
        }

        [Fact]
        public void BuildParameters_FromStruct_Properties()
        {
            var value = new CustomStructWithProperties() { IntProperty = 2, StringProperty = "test" };

            var bindings = ParameterBinder.BuildParameterBindings(value);

            Assert.True(bindings.ContainsKey(nameof(value.IntProperty)));
            Assert.True(bindings[nameof(value.IntProperty)].Type == "FIXED");
            Assert.True(bindings[nameof(value.IntProperty)].Value == value.IntProperty.ToString());

            Assert.True(bindings.ContainsKey(nameof(value.StringProperty)));
            Assert.True(bindings[nameof(value.StringProperty)].Type == "TEXT");
            Assert.True(bindings[nameof(value.StringProperty)].Value == value.StringProperty);
        }

        [Fact]
        public void BuildParameters_FromClass_Fields()
        {
            var value = new CustomClassWithFields() { IntField = 2, StringField = "test" };

            var bindings = ParameterBinder.BuildParameterBindings(value);

            Assert.True(bindings.ContainsKey(nameof(value.IntField)));
            Assert.True(bindings[nameof(value.IntField)].Type == "FIXED");
            Assert.True(bindings[nameof(value.IntField)].Value == value.IntField.ToString());

            Assert.True(bindings.ContainsKey(nameof(value.StringField)));
            Assert.True(bindings[nameof(value.StringField)].Type == "TEXT");
            Assert.True(bindings[nameof(value.StringField)].Value == value.StringField);
        }

        [Fact]
        public void BuildParameters_FromStruct_Fields()
        {
            var value = new CustomStructWithFields() { IntField = 2, StringField = "test" };

            var bindings = ParameterBinder.BuildParameterBindings(value);

            Assert.True(bindings.ContainsKey(nameof(value.IntField)));
            Assert.True(bindings[nameof(value.IntField)].Type == "FIXED");
            Assert.True(bindings[nameof(value.IntField)].Value == value.IntField.ToString());

            Assert.True(bindings.ContainsKey(nameof(value.StringField)));
            Assert.True(bindings[nameof(value.StringField)].Type == "TEXT");
            Assert.True(bindings[nameof(value.StringField)].Value == value.StringField);
        }

        [Fact]
        public void BuildParameters_FromAnonymousType()
        {
            var value = new { IntProperty = 2, StringProperty = "test" };

            var bindings = ParameterBinder.BuildParameterBindings(value);

            Assert.True(bindings.ContainsKey(nameof(value.IntProperty)));
            Assert.True(bindings[nameof(value.IntProperty)].Type == "FIXED");
            Assert.True(bindings[nameof(value.IntProperty)].Value == value.IntProperty.ToString());

            Assert.True(bindings.ContainsKey(nameof(value.StringProperty)));
            Assert.True(bindings[nameof(value.StringProperty)].Type == "TEXT");
            Assert.True(bindings[nameof(value.StringProperty)].Value == value.StringProperty);
        }
        private class CustomClassWithProperties
        {
            public string StringProperty { get; set; }

            public int IntProperty { get; set; }
        }

        private class CustomClassWithFields
        {
            public string StringField;

            public int IntField;
        }

        private struct CustomStructWithProperties
        {
            public string StringProperty { get; set; }

            public int IntProperty { get; set; }
        }

        private struct CustomStructWithFields
        {
            public string StringField;

            public int IntField;
        }
    }
}
