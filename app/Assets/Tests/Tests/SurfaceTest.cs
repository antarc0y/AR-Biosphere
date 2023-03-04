using NUnit.Framework;

public class SurfaceTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void SurfaceTestSimplePasses()
    {
        var surface = new Surface();
        Assert.IsTrue(surface.IsLand());
        surface.ChangeSurface();
        Assert.IsTrue(surface.IsWater());
    }
}
