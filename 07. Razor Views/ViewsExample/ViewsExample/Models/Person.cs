namespace ViewsExample.Models
{
    public class Person
    {
        public string? Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Enum? PersonGender { get; set; }
    }

    public enum Gender
    {
        Male, Female, Other
    }
}
