using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Repository.Models;

namespace velowrench.Repository.Catalogs;

public static class HardCodedComponentStandardCatalog
{
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

    public static ReadOnlyCollection<WheelSpecificationModel> GetAllWheelSpecifications => new(
    [
        new("10 × 1 5/8 (10'')",7.638 ,194),
        new("12½'' (juvenile)",7.992  ,203),
        new("",8.740  ,222),
        new("12 × 1 3/8",9.409  ,239),
        new("12 × 1 1/4",9.764  ,248),
        new("",9.882  ,251),
        new("hooked-bead 270 (beaded-edge)",10.630  ,270),
        new("14'' (350)",10.984  ,279),
        new("14'' (350 A)",11.339  ,288),
        new("14'' variants",11.732  ,298),
        new("16'' (decimal)",12.008  ,305),
        new("",12.480  ,317),
        new("hooked-bead 321",12.638  ,321),
        new("16'' / 400×38B",12.992  ,330),
        new("",13.268  ,337),
        new("16'' (400A)",13.386  ,340),
        new("16'' variants (400×32A)",13.740  ,349),
        new("18'' (355)",13.976  ,355),
        new("17''",14.055  ,357),
        new("17'' variants",14.528  ,369),
        new("hooked-bead 372",14.646  ,372),
        new("",15.000  ,381),
        new("18'' variants",15.236  ,387),
        new("18'' / 450A",15.354  ,390),
        new("18'' / 450A variants",15.748  ,400),
        new("20'' (BMX / common 20'')",15.984  ,406),
        new("",16.496  ,419),
        new("hooked-bead 422",16.614  ,422),
        new("20'' variant (500A)",16.850  ,428),
        new("20 × 1.5 (432)",17.008  ,432),
        new("20 × 1 3/8",17.244  ,438),
        new("20 × 1 3/8 / 500A",17.323  ,440),
        new("20 × 1 1/8 (451)",17.756  ,451),
        new("22''",17.992  ,457),
        new("hooked-bead 459",18.071  ,459),
        new("hooked-bead 473",18.622  ,473),
        new("",19.055  ,484),
        new("22 × 1 3/8 variants",19.252  ,489),
        new("22 × 1 3/8 (550A)",19.291  ,490),
        new("22 × 1 3/8 × 1 1/4",19.606  ,498),
        new("22 × 1 3/8",19.724  ,501),
        new("24'' (507)",19.961  ,507),
        new("hooked-bead 510",20.079  ,510),
        new("24 × 1 (520)",20.472  ,520),
        new("hooked-bead 524",20.630  ,524),
        new("",20.906  ,531),
        new("",21.024  ,534),
        new("24 × 1 3/8 (540)",21.260  ,540),
        new("24 × 1 3/8 (older) / 600A",21.299  ,541),
        new("24 × 1 1/4 (Schwinn 547)",21.535  ,547),
        new("26'' (559) — common MTB/cruiser",22.008  ,559),
        new("hooked-bead 560",22.047  ,560),
        new("25'' / 565 variants",22.244  ,565),
        new("26'' (571) Schwinn/older",22.480  ,571),
        new("hooked-bead 575",22.638  ,575),
        new("27.5'' / 650B",22.992  ,584),
        new("26 × 1 3/8 (650A)",23.228  ,590),
        new("26 × 1 1/4 (597)",23.504  ,597),
        new("",23.976  ,609),
        new("hooked-bead 611",24.055  ,611),
        new("700C / 29'' (622)",24.488  ,622),
        new("27'' (630)",24.803  ,630),
        new("700B / 28 × 1½ (635)",25.000  ,635),
        new("700A (older)",25.276  ,642),
        new("32'' (unicycle / tall-bike)",27.008  ,686),
        new("36'' (unicycle / 36er)",30.984 ,787)
    ]);

    public static ReadOnlyCollection<WheelSpecificationModel> GetMostCommonWheelSpecifications => new(
    [
        new("16'' / 400×38B",12.992  ,330),
        new("16'' (400A)",13.386  ,340),
        new("17''",14.055  ,357),
        new("18'' (355)",13.976  ,355),
        new("18'' / 450A",15.354  ,390),
        new("20'' (BMX)",15.984  ,406),
        new("22''",17.992  ,457),
        new("24'' (507)",19.961  ,507),
        new("24 × 1 (520)",20.472  ,520),
        new("26'' (559) — common MTB/cruiser",22.008  ,559),
        new("26 × 1 3/8 (650A)",23.228  ,590),
        new("26 × 1 1/4 (597)",23.504  ,597),
        new("27.5'' / 650B",22.992  ,584),
        new("700C / 29'' (622)",24.488  ,622),
        new("27'' (630)",24.803  ,630),
        new("700B / 28 × 1½ (635)",25.000  ,635),
        new("700A (older)",25.276  ,642),
        new("32'' (unicycle / tall-bike)",27.008  ,686),
        new("36'' (unicycle / 36er)",30.984 ,787)
    ]);

    public static ReadOnlyCollection<CadenceModel> GetAllCandences => new(
    [
        new("40 rpm", 40),
        new("60 rpm", 60),
        new("80 rpm", 80),
        new("90 rpm", 90),
        new("100 rpm", 100),
        new("120 rpm", 120)
    ]);
}