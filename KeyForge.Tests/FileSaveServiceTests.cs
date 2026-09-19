using Random_password_generator.Services;

namespace KeyForge.Tests;

public class FileSaveServiceTests
{
    [Theory]
    [InlineData("..")]
    [InlineData("../../evil")]
    [InlineData("..\\..\\evil")]
    public void Save_RejectsDirectoryTraversalOutsideDesktop(string directoryName)
    {
        (bool success, string message) = FileSaveService.Save("does-not-matter", directoryName);

        Assert.False(success);
        Assert.NotEmpty(message);
    }
}
