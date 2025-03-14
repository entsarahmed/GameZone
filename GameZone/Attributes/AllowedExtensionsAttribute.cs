namespace GameZone.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        //Start Add Validation to need it 
        private readonly string _allowedExtensions;
        public AllowedExtensionsAttribute( string allowedExtension)
        {
            _allowedExtensions = allowedExtension;
        }
        protected override ValidationResult? IsValid
            (object? value, ValidationContext validationContext)
        {
            //convert file to IFormFile
            var file = value as IFormFile;
            if (file is not null)
            {
                // Send file and return Extension
                var extension = Path.GetExtension(file.FileName);
                var isAllowed = _allowedExtensions.Split(',').Contains(extension, StringComparer.OrdinalIgnoreCase);
                if (!isAllowed)
                {
                    return new ValidationResult($"Only {_allowedExtensions} are allowed!");
                }
            }
            return ValidationResult.Success;
        }
    }
}
