RubberStamp
=================
[![Build status](https://ci.appveyor.com/api/projects/status/h3nh3ibim15910ak/branch/master?svg=true)](https://ci.appveyor.com/project/chriswalpen/rubberstamp/branch/master)

RubberStamp is a very simple Object Validation Framework for .NET
The Fluent Synthax makes it intuitive and easy to use in any Application or component.

A validation object is created for one type.
Each validator can have multpile rules
- A rule is generaly a validation of one Property
Each rule can have multiple conditions that have to apply
- A condintion is a check that is applied to the Property assigned to the rule

### Dynamic validation
```csharp
var validator = new Validator<TestClass>()
    .AddRule(p => p.Name, con => con.IsNotNull(), ext => ext.SetMessage("This is a custom message"))
    .AddRule(p => p.Index, con => con.GreaterThan(5));

var summary = validator.Validate(new TestClass { Name = "test", Index = 6 });
Assert.That(summary.IsValid);
```

### Dedicated validation
```csharp
private class PersonValidator : Validator<Person>
{
    public PersonValidator()
    {
        AddRule(p => p.Name, con => con.IsNotNull(), ext => ext.When(p => p.Firstname != null), ext => ext.SetMessage("Name is not allowed to be null when firstname is not null"));
    }
}
```

### Validation in Validate override 
```csharp
private class PersonValidator : Validator<Person>
{
    protected override void Validate(IValidationSummary summary, Person item)
    {
        if (item.Name == "Taylor" && item.Firstname == "Tim")
        {
            summary.AddResult(new ValidationResult(Severity.Error, "Tim Taylor is not alowed"));
        }

        base.Validate(summary, item);
    }
}
```


