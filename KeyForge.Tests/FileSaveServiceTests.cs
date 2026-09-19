using Random_password_generator.Services;

namespace KeyForge.Tests;

public class FileSaveServiceTests
{
    [Theory]
    [InlineData("..")]
    [InlineData("../../evil")]
    public void Save_RejectsDirectoryTraversalOutsideDesktop(string directoryName)
    {
        (bool success, string message) = FileSaveService.Save("does-not-matter", directoryName);

        Assert.False(success);
        Assert.NotEmpty(message);
    }

    [Fact]
    public void Save_RejectsBackslashDirectoryTraversalOutsideDesktop_OnWindows()
    {
        // '\' is only a path separator on Windows; on Unix it's just a literal
        // character in a single path segment, so this case is Windows-specific.
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        (bool success, string message) = FileSaveService.Save("does-not-matter", "..\\..\\evil");

        Assert.False(success);
        Assert.NotEmpty(message);
    }
}
