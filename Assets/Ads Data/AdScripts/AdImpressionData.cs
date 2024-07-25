public struct AdImpressionData
{
    public int Index;
    public string AdUnit;
    public AdFormat Format;

    public AdImpressionData(int index, string adUnit, AdFormat format)
    {
        Index = index;
        AdUnit = adUnit;
        Format = format;
    }
}