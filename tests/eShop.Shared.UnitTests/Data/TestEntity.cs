using System.ComponentModel.DataAnnotations;
using eShop.Shared.Data;

namespace eShop.Shared.UnitTests.Data;

internal class TestEntity : Entity
{
    public void SetId(int id) => this.Id = id;
}
