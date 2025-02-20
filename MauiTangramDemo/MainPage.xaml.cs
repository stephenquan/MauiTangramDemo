using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace MauiTangramDemo;

public partial class MainPage : ContentPage
{
    SKBitmap? tangramzones;
    static Dictionary<Color, string> tangramdict = new Dictionary<Color, string>
    {
        { Color.FromArgb("#FF7476"), "Large Left Triangle" },
        { Color.FromArgb("#DA9AFF"), "Large Top Triangle" },
        { Color.FromArgb("#8994FF"), "Small Middle Triangle" },
        { Color.FromArgb("#2DD998"), "Small Bottom Triangle" },
        { Color.FromArgb("#FF8902"), "Middle Triangle" },
        { Color.FromArgb("#65FFCC"), "Square" },
        { Color.FromArgb("#FFCD1F"), "Parallelogram" },
    };

    SKBitmap? manzones;
    static Dictionary<Color, string> mandict = new Dictionary<Color, string>
    {
        { Color.FromArgb("#65FFCC"), "Head" },
        { Color.FromArgb("#DA9AFF"), "Chest" },
        { Color.FromArgb("#FF7476"), "Stomach" },
        { Color.FromArgb("#FFCD1F"), "Top leg" },
        { Color.FromArgb("#2DD998"), "Top foot" },
        { Color.FromArgb("#FF8902"), "Bottom leg" },
        { Color.FromArgb("#8994FF"), "Bottom foot" },
    };

    public MainPage()
    {
        InitializeComponent();
        Dispatcher.Dispatch(async () =>
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("tangram_zones.png");
            tangramzones = SKBitmap.Decode(stream);
            var manstream = await FileSystem.OpenAppPackageFileAsync("tangram_man_zones.png");
            manzones = SKBitmap.Decode(manstream);
        });
    }

    /// <summary>
    /// Demonstrates detection of a user click.
    /// </summary>
    /// <param name="view"></param>
    /// <param name="getPosition"></param>
    /// <param name="zones"></param>
    /// <param name="dict"></param>
    /// <param name="targetLabel"></param>
    public void ImageDetector(View view, Func<Element?, Point?> getPosition, SKBitmap? zones, Dictionary<Color, string> dict, Label targetLabel)
    {
        // Determine the pixel color of the tap
        if (zones is null) return;
        Point? pos = getPosition(view);
        if (pos is null) return;
        int normalizedX = (int)(pos.Value.X * zones.Width / view.Width);
        int normalizedY = (int)(pos.Value.Y * zones.Height / view.Height);
        SKColor skcolor = zones.GetPixel(normalizedX, normalizedY);
        Color color = skcolor.ToMauiColor();
        // Determine the item at the tap location
        if (dict.ContainsKey(color))
        {
            string item = dict[color];
            targetLabel.Text = $"{item} found at {normalizedX},{normalizedY}";
        }
        else
        {
            targetLabel.Text = $"No item found at {normalizedX},{normalizedY} with color {color.ToHex()}";
        }
    }

    /// <summary>
    /// Demonstrates detection of a user click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void IdentifyTangramPieces(object sender, TappedEventArgs e)
        => ImageDetector((View)sender, e.GetPosition, tangramzones, tangramdict, tangramInfo);

    /// <summary>
    /// Demonstrates detection of a user click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void IdentifyMan(object sender, TappedEventArgs e)
        => ImageDetector((View)sender, e.GetPosition, manzones, mandict, manInfo);

    /// <summary>
    /// Demonstrates detection of a user hover.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HoverTangramPieces(object sender, PointerEventArgs e)
        => ImageDetector((View)sender, e.GetPosition, tangramzones, tangramdict, tangramInfo);

    /// <summary>
    /// Demonstrates detection of a user hover.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HoverMan(object sender, PointerEventArgs e)
        => ImageDetector((View)sender, e.GetPosition, manzones, mandict, manInfo);
}
