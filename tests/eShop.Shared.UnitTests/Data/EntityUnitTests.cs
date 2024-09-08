using MediatR;

namespace eShop.Shared.UnitTests.Data;

public class EntityUnitTests
{
    [Theory, AutoNSubstituteData]
    internal void entity_should_have_default_id(
        TestEntity entity)
    {
        // Arrange

        // Act

        // Assert

        Assert.Equal(default, entity.Id);
    }

    [Theory, AutoNSubstituteData]
    internal void set_id(
        TestEntity entity,
        int id)
    {
        // Arrange        

        // Act

        entity.SetId(id);

        // Assert

        Assert.Equal(id, entity.Id);
    }

    [Theory, AutoNSubstituteData]
    internal void clear_events(
        INotification notification,
        TestEntity entity)
    {
        // Arrange

        entity.AddDomainEvent(notification);

        // Act

        entity.ClearDomainEvents();

        // Assert

        Assert.Empty(entity.DomainEvents!);
    }

    public class IsTransient
    {
        [Theory, AutoNSubstituteData]
        internal void transient_entity(
            TestEntity entity)
        {
            // Arrange
            // Act
            // Assert
            Assert.True(entity.IsTransient());
        }
        [Theory, AutoNSubstituteData]
        internal void not_transient_entity(
            TestEntity entity,
            int id)
        {
            // Arrange
            entity.SetId(id);
            // Act
            // Assert
            Assert.False(entity.IsTransient());
        }
    }

    public class EqualsTests
    {
        [Theory, AutoNSubstituteData]
        internal void two_entities_are_equal(
            TestEntity entity)
        {
            // Arrange
            TestEntity copy = entity;
            // Act
            bool equal = entity.Equals(copy);
            // Assert
            Assert.True(equal);
        }
        [Theory, AutoNSubstituteData]
        internal void obj_is_null_return_false(
            TestEntity entity)
        {
            // Arrange
            // Act
            bool equal = entity.Equals(null);
            // Assert
            Assert.False(equal);
        }

        [Theory, AutoNSubstituteData]
        internal void when_types_are_different_return_false(
            TestEntity entity,
            OtherEntity otherEntity)
        {
            // Arrange
            // Act
            bool equal = entity.Equals(otherEntity);
            // Assert
            Assert.False(equal);
        }

        [Theory, AutoNSubstituteData]
        internal void when_entity2_is_transient_return_false(
            TestEntity entity,
            TestEntity entity2)
        {
            // Arrange

            entity2.SetId(0);

            // Act

            bool equal = entity.Equals(entity2);

            // Assert
            Assert.False(equal);
        }

        [Theory, AutoNSubstituteData]
        internal void when_entity1_is_transient_return_false(
            TestEntity entity1,
            TestEntity entity2)
        {
            // Arrange

            entity1.SetId(0);

            // Act

            bool equal = entity1.Equals(entity2);

            // Assert
            Assert.False(equal);
        }
    }

    public class GetHashCodeTests
    {
        [Theory, AutoNSubstituteData]
        internal void when_transient_return_get_hash_code(
            TestEntity entity)
        {
            // Arrange

            // Act

            int result = entity.GetHashCode();

            // Assert

            Assert.NotEqual(0, result);
        }

        [Theory, AutoNSubstituteData]
        internal void return_same_hash_code(
            TestEntity entity)
        {
            // Arrange

            // Act

            int hashCode1 = entity.GetHashCode();
            int hashCode2 = entity.GetHashCode();

            // Assert

            Assert.Equal(hashCode1, hashCode2);
        }
    }
}
