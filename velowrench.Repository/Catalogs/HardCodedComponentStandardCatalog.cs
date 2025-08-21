using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Repository.Models;

namespace velowrench.Repository.Catalogs;

/// <summary>
/// Provides temporary static collections of standard bicycle component specifications for use throughout the application.
/// </summary>
public static class HardCodedComponentStandardCatalog
{
    /// <summary>
    /// Gets a collection of all available crankset specifications.
    /// </summary>
    public static ReadOnlyCollection<CranksetSpecificationModel> GetAllCranksetSpecifications => new(
    [
        new("100 mm", 100),
        new("105 mm", 105),
        new("110 mm", 110),
        new("115 mm", 115),
        new("120 mm", 120),
        new("125 mm", 125),
        new("130 mm", 130),
        new("135 mm", 135),
        new("140 mm", 140),
        new("145 mm", 145),
        new("150 mm", 150),
        new("155 mm", 155),
        new("160 mm", 160),
        new("165 mm", 165),
        new("170 mm", 170),
        new("175 mm", 175),
        new("180 mm", 180),
        new("185 mm", 185),
        new("190 mm", 190),
    ]);

    /// <summary>
    /// Gets a collection of all available wheel specifications.
    /// </summary>
    public static ReadOnlyCollection<WheelSpecificationModel> GetAllWheelSpecifications => new(
    [
        new(label: "10 × 1 5/8 (10'')", bSDin:7.638, tyreHeightIn:1.3),
        new(label: "12½'' (juvenile)", bSDin:7.992, tyreHeightIn:2.1),
        new(label: "", bSDin:8.740, tyreHeightIn:1.4),
        new(label: "12 × 1 3/8", bSDin:9.409, tyreHeightIn:1.25),
        new(label: "12 × 1 1/4", bSDin:9.764, tyreHeightIn:1.25),
        new(label: "", bSDin:9.882, tyreHeightIn:1.4),
        new(label: "hooked-bead 270 (beaded-edge)", bSDin:10.630, tyreHeightIn:1.5),
        new(label: "14'' (350)", bSDin:10.984, tyreHeightIn:1.5),
        new(label: "14'' (350 A)", bSDin:11.339, tyreHeightIn:1.5),
        new(label: "14'' variants", bSDin:11.732, tyreHeightIn:1.5),
        new(label: "16'' (decimal)", bSDin:12.008, tyreHeightIn:2.1),
        new(label: "", bSDin:12.480, tyreHeightIn:1.5),
        new(label: "hooked-bead 321", bSDin:12.638, tyreHeightIn:1.5),
        new(label: "16'' / 400×38B", bSDin:12.992, tyreHeightIn:1.75),
        new(label: "", bSDin:13.268, tyreHeightIn:1.6),
        new(label: "16'' (400A)", bSDin:13.386, tyreHeightIn:1.75),
        new(label: "16'' variants (400×32A)", bSDin:13.740, tyreHeightIn:1.5),
        new(label: "18'' (355)", bSDin:13.976, tyreHeightIn:2.0),
        new(label: "17''", bSDin:14.055, tyreHeightIn:2.0),
        new(label: "17'' variants", bSDin:14.528, tyreHeightIn:2.0),
        new(label: "hooked-bead 372", bSDin:14.646, tyreHeightIn:1.9),
        new(label: "", bSDin:15.000, tyreHeightIn:2.1),
        new(label: "18'' variants", bSDin:15.236, tyreHeightIn:1.9),
        new(label: "18'' / 450A", bSDin:15.354, tyreHeightIn:1.3),
        new(label: "18'' / 450A variants", bSDin:15.748, tyreHeightIn:1.3),
        new(label: "20'' (BMX / common 20'')", bSDin:15.984, tyreHeightIn:2.2),
        new(label: "", bSDin:16.496, tyreHeightIn:2.1),
        new(label: "hooked-bead 422", bSDin:16.614, tyreHeightIn:1.75),
        new(label: "20'' variant (500A)", bSDin:16.850, tyreHeightIn:1.25),
        new(label: "20 × 1.5 (432)", bSDin:17.008, tyreHeightIn:1.5),
        new(label: "20 × 1 3/8", bSDin:17.244, tyreHeightIn:1.375),
        new(label: "20 × 1 3/8 / 500A", bSDin:17.323, tyreHeightIn:1.375),
        new(label: "20 × 1 1/8 (451)", bSDin:17.756, tyreHeightIn:1.125),
        new(label: "22''", bSDin:17.992, tyreHeightIn:2.0),
        new(label: "hooked-bead 459", bSDin:18.071, tyreHeightIn:1.75),
        new(label: "hooked-bead 473", bSDin:18.622, tyreHeightIn:1.75),
        new(label: "", bSDin:19.055, tyreHeightIn:1.6),
        new(label: "22 × 1 3/8 variants", bSDin:19.252, tyreHeightIn:1.375),
        new(label: "22 × 1 3/8 (550A)", bSDin:19.291, tyreHeightIn:1.375),
        new(label: "22 × 1 3/8 × 1 1/4", bSDin:19.606, tyreHeightIn:1.3),
        new(label: "22 × 1 3/8", bSDin:19.724, tyreHeightIn:1.375),
        new(label: "24'' (507)", bSDin:19.961, tyreHeightIn:2.1),
        new(label: "hooked-bead 510", bSDin:20.079, tyreHeightIn:1.75),
        new(label: "24 × 1 (520)", bSDin:20.472, tyreHeightIn:1.0),
        new(label: "hooked-bead 524", bSDin:20.630, tyreHeightIn:1.75),
        new(label: "", bSDin:20.906, tyreHeightIn:1.6),
        new(label: "", bSDin:21.024, tyreHeightIn:1.6),
        new(label: "24 × 1 3/8 (540)", bSDin:21.260, tyreHeightIn:1.375),
        new(label: "24 × 1 3/8 (older) / 600A", bSDin:21.299, tyreHeightIn:1.375),
        new(label: "24 × 1 1/4 (Schwinn 547)", bSDin:21.535, tyreHeightIn:1.25),
        new(label: "26'' (559) — common MTB/cruiser", bSDin:22.008, tyreHeightIn:2.25),
        new(label: "hooked-bead 560", bSDin:22.047, tyreHeightIn:2.25),
        new(label: "25'' / 565 variants", bSDin:22.244, tyreHeightIn:1.75),
        new(label: "26'' (571) Schwinn/older", bSDin:22.480, tyreHeightIn:2.0),
        new(label: "hooked-bead 575", bSDin:22.638, tyreHeightIn:1.75),
        new(label: "27.5'' / 650B", bSDin:22.992, tyreHeightIn:2.25),
        new(label: "26 × 1 3/8 (650A)", bSDin:23.228, tyreHeightIn:1.375),
        new(label: "26 × 1 1/4 (597)", bSDin:23.504, tyreHeightIn:1.25),
        new(label: "", bSDin:23.976, tyreHeightIn:1.6),
        new(label: "hooked-bead 611", bSDin:24.055, tyreHeightIn:1.75),
        new(label: "700C / 29'' (622)", bSDin:24.488, tyreHeightIn:1.05),
        new(label: "27'' (630)", bSDin:24.803, tyreHeightIn:1.0),
        new(label: "700B / 28 × 1½ (635)", bSDin:25.000, tyreHeightIn:1.5),
        new(label: "700A (older)", bSDin:25.276, tyreHeightIn:1.1),
        new(label: "32'' (unicycle / tall-bike)", bSDin:27.008, tyreHeightIn:2.25),
        new(label: "36'' (unicycle / 36er)", bSDin:30.984, tyreHeightIn:2.25)
    ]);

    /// <summary>
    /// Gets a collection of the most commonly used wheel specifications.
    /// </summary>
    public static ReadOnlyCollection<WheelSpecificationModel> GetMostCommonWheelSpecifications => new(
    [
        new(label: "16'' / 400×38B", bSDin:12.992, tyreHeightIn:1.75),
        new(label: "16'' (400A)", bSDin:13.386, tyreHeightIn:1.75),
        new(label: "17''", bSDin:14.055, tyreHeightIn:2.0),
        new(label: "18'' (355)", bSDin:13.976, tyreHeightIn:2.0),
        new(label: "18'' / 450A", bSDin:15.354, tyreHeightIn:1.3),
        new(label: "20'' (BMX)", bSDin:15.984, tyreHeightIn:2.2),
        new(label: "22''", bSDin:17.992, tyreHeightIn:2.0),
        new(label: "24'' (507)", bSDin:19.961, tyreHeightIn:2.1),
        new(label: "24 × 1 (520)", bSDin:20.472, tyreHeightIn:1.0),
        new(label: "26'' (559 - common MTB/cruiser)", bSDin:22.008, tyreHeightIn:2.25),
        new(label: "26 × 1 3/8 (650A)", bSDin:23.228, tyreHeightIn:1.375),
        new(label: "26 × 1 1/4 (597)", bSDin:23.504, tyreHeightIn:1.25),
        new(label: "27.5'' / 650B", bSDin:22.992, tyreHeightIn:2.25),
        new(label: "700C / 29'' (622)", bSDin:24.488, tyreHeightIn:1.05),
        new(label: "27'' (630)", bSDin:24.803, tyreHeightIn:1.0),
        new(label: "700B / 28 × 1½ (635)", bSDin:25.000, tyreHeightIn:1.5),
        new(label: "700A (older)", bSDin:25.276, tyreHeightIn:1.1),
        new(label: "32'' (unicycle/tall-bike)", bSDin:27.008, tyreHeightIn:2.25),
        new(label: "36'' (unicycle/36er)", bSDin:30.984, tyreHeightIn:2.25)
    ]);

    /// <summary>
    /// Gets a collection of commonly used cadence values.
    /// </summary>
    public static ReadOnlyCollection<CadenceModel> GetMostUsedCandences => new(
    [
        new("40 rpm", 40),
        new("60 rpm", 60),
        new("80 rpm", 80),
        new("90 rpm", 90),
        new("100 rpm", 100),
        new("120 rpm", 120)
    ]);

    /// <summary>
    /// Gets a comprehensive collection of standard sprocket specifications.
    /// </summary>
    public static ReadOnlyCollection<SprocketSpecificationModel> GetMostCommonSprocketSpecification => new(
    [
        new("9", 9),
        new("10", 10),
        new("11", 11),
        new("12", 12),
        new("13", 13),
        new("14", 14),
        new("15", 15),
        new("16", 16),
        new("17", 17),
        new("18", 18),
        new("19", 19),
        new("20", 20),
        new("21", 21),
        new("22", 22),
        new("23", 23),
        new("24", 24),
        new("25", 25),
        new("26", 26),
        new("27", 27),
        new("28", 28),
        new("29", 29),
        new("30", 30),
        new("31", 31),
        new("32", 32),
        new("33", 33),
        new("34", 34),
        new("35", 35),
        new("36", 36),
        new("37", 37),
        new("38", 38),
        new("39", 39),
        new("40", 40),
        new("41", 41),
        new("42", 42),
        new("43", 43),
        new("44", 44),
        new("45", 45),
        new("46", 46),
        new("47", 47),
        new("48", 48),
        new("49", 49),
        new("50", 50),
        new("51", 51),
        new("52", 52)
    ]);
}