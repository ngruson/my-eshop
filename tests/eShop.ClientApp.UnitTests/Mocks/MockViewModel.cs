using eShop.ClientApp.Validations;
using eShop.ClientApp.ViewModels.Base;

namespace eShop.ClientApp.UnitTests.Mocks;

public class MockViewModel : ViewModelBase
{
    public ValidatableObject<string> Forename { get; } = new();

    public ValidatableObject<string> Surname { get; } = new();

    public MockViewModel(INavigationService navigationService)
        : base(navigationService)
    {
        this.Forename = new ValidatableObject<string>();
        this.Surname = new ValidatableObject<string>();

        this.Forename.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Forename is required." });
        this.Surname.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Surname name is required." });
    }

    public bool Validate()
    {
        bool isValidForename = this.Forename.Validate();
        bool isValidSurname = this.Surname.Validate();
        return isValidForename && isValidSurname;
    }
}
