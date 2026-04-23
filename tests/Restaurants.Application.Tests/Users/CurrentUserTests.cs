namespace Restaurants.Application.Tests.Users;

public class CurrentUserTests
{
    /// <summary>
    /// Tests that the IsInRole method returns true when the user has the specified role.
    /// </summary>
    [Fact]
    public void IsInRole_WithMatchingRole_ReturnsTrue()
    {
        // Arrange
        var user = new CurrentUser("1",
                                   "test@test.com",
                                   [UserRoles.Admin, UserRoles.User],
                                   null,
                                   null);

        // Act
        var result = user.IsInRole(UserRoles.Admin);

        // Assert
        result.Should().BeTrue();
    }


    /// <summary>
    /// Tests that the IsInRole method returns false when the user does not have the specified role.
    /// </summary>
    [Fact]
    public void IsInRole_WithNonMatchingRole_ReturnsFalse()
    {
        // Arrange
        var user = new CurrentUser("1",
                                   "test@test.com",
                                   [UserRoles.Admin, UserRoles.User],
                                   null,
                                   null);

        // Act
        var result = user.IsInRole(UserRoles.Owner);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that the IsInRole method returns false when the role case does not match, demonstrating that role checks are case-sensitive.
    /// </summary>
    [Fact]
    public void IsInRole_WithNonMatchingRoleCase_ReturnsFalse()
    {
        // Arrange
        var user = new CurrentUser("1",
                                   "test@test.com",
                                   [UserRoles.Admin, UserRoles.User],
                                   null,
                                   null);

        // Act
        var result = user.IsInRole(UserRoles.Admin.ToLower());

        // Assert
        result.Should().BeFalse();
    }
}