namespace BSolutions.Brecons.Core.Exceptions
{
    using System;

    public class MandatoryAttributeException : Exception
    {
        public string Attribute { get; set; }

        public Type TagHelper { get; set; }

        public MandatoryAttributeException(string attribute)
            : base($"The '{attribute}' attribute is mandatory and must be set.")
        {
            this.Attribute = attribute;
        }

        public MandatoryAttributeException(string attribute, Type tagHelper)
            : base($"The '{attribute}' attribute of the '{tagHelper.Name}' is mandatory and must be set.")
        {
            this.Attribute = attribute;
            this.TagHelper = tagHelper;
        }
    }
}