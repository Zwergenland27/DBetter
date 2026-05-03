using DBetter.Domain.MultipleUnitVehicleSeriesVariant.ValueObjects;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.MultipleUnitVehicleSeriesVariant;

public static class KnownMultipleUnitVariants
{
    public static MultipleUnitVehicleSeriesVariant Br401Ldv => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("7afeb91d-580d-4948-affe-a27229085ee7").Value,
        new MultipleUnitVariantIdentifier("401", "LDV"),
        PowerType.Electric,
        new SpeedLimit(280));
    
    public static MultipleUnitVehicleSeriesVariant Br402Default => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("fe338b16-cf75-4315-8fae-ff60a1f33ad3").Value,
        MultipleUnitVariantIdentifier.Default("402"),
        PowerType.Electric,
        new SpeedLimit(280));
    
    public static MultipleUnitVehicleSeriesVariant Br403Redesign => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("3e71e7b8-c996-4631-b0a7-a3add6b14bc5").Value,
        new MultipleUnitVariantIdentifier("403", "R"),
        PowerType.Electric,
        new SpeedLimit(320));
    
    public static MultipleUnitVehicleSeriesVariant Br403FirstSeries => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("2c82b05a-aff5-4587-bdd1-bc993dc51ab8").Value,
        new MultipleUnitVariantIdentifier("403", "S1"),
        PowerType.Electric,
        new SpeedLimit(320));
    
    public static MultipleUnitVehicleSeriesVariant Br403SecondSeries => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("d07e2775-db53-47af-ba31-5f3fbf0a10c2").Value,
        new MultipleUnitVariantIdentifier("403", "S2"),
        PowerType.Electric,
        new SpeedLimit(320));
    
    public static MultipleUnitVehicleSeriesVariant Br406 => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("a80524c0-0bde-456c-901d-5a081727e4ef").Value,
        MultipleUnitVariantIdentifier.Default("406"),
        PowerType.Electric,
        new SpeedLimit(320));
    
    public static MultipleUnitVehicleSeriesVariant Br406Redesign => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("9dbee8ab-7feb-4b69-bc0e-e368d806aba0").Value,
        new MultipleUnitVariantIdentifier("406", "R"),
        PowerType.Electric,
        new SpeedLimit(320));
    
    public static MultipleUnitVehicleSeriesVariant Br407Default => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("4cfb26b6-1e59-4d56-8496-bc9a66dd6b2d").Value,
        MultipleUnitVariantIdentifier.Default("407"),
        PowerType.Electric,
        new SpeedLimit(320));
    
    public static MultipleUnitVehicleSeriesVariant Br408Default => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("50b9c1e1-5b50-4205-8925-237c94a959a6").Value,
        MultipleUnitVariantIdentifier.Default("408"),
        PowerType.Electric,
        new SpeedLimit(320));
    
    public static MultipleUnitVehicleSeriesVariant Br411FirstSeries => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("b24c9562-7058-4bcb-9585-078d8bafc550").Value,
        new MultipleUnitVariantIdentifier("411", "S1"),
        PowerType.Electric,
        new SpeedLimit(230));
    
    public static MultipleUnitVehicleSeriesVariant Br411SecondSeries => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("3dc8964b-e7c1-4227-b7ea-20a18df72751").Value,
        new MultipleUnitVariantIdentifier("411","S2"),
        PowerType.Electric,
        new SpeedLimit(230));
    
    public static MultipleUnitVehicleSeriesVariant Br412_7 => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("93dff733-7a8b-4127-8ba9-a50491e64630").Value,
        new MultipleUnitVariantIdentifier("412", "7"),
        PowerType.Electric,
        new SpeedLimit(260));
    
    public static MultipleUnitVehicleSeriesVariant Br412_12 => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("957e4578-108b-4fa6-bac6-399d833114bd").Value,
        new MultipleUnitVariantIdentifier("412", "12"),
        PowerType.Electric,
        new SpeedLimit(260));
    
    public static MultipleUnitVehicleSeriesVariant Br412_13 => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("ebae4f7d-335a-4d30-b2a4-46f13dde6858").Value,
        new MultipleUnitVariantIdentifier("412", "13"),
        PowerType.Electric,
        new SpeedLimit(260));
    
    public static MultipleUnitVehicleSeriesVariant Br415Default => new MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId.Create("120bd498-558d-4eac-b687-1cd5c719bba0").Value,
        MultipleUnitVariantIdentifier.Default("415"),
        PowerType.Electric,
        new SpeedLimit(230));
}