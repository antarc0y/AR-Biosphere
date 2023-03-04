using NUnit.Framework;

public class SurfaceTest
{
    // Test Surface script
    [Test]
    public void SurfaceTestSimplePasses()
    {
        // Test surface is land by default and can be changed to water
        var surface = new Surface();
        Assert.IsTrue(surface.IsLand());
        surface.ChangeSurface();
        Assert.IsTrue(surface.IsWater());
    }
}
