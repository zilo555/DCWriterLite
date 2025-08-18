using System;
using System.Collections.Generic;
using System.Text;
// // 
//using System.Drawing.Printing;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 提供标准的页面大小的信息对象
    /// </summary>
    public static class StandardPaperSizeInfo
    {
        public static DCSystem_Drawing.Size GetStandardPaperSize(PaperKind kind)
        {
            int index = (int)kind;
            if (index >= 0 && index < myStandardPaperSize.Length)
            {
                return myStandardPaperSize[index];
            }
            else
            {
                return DCSystem_Drawing.Size.Empty;
            }
        }

        private readonly static DCSystem_Drawing.Size[] myStandardPaperSize = null;
        static StandardPaperSizeInfo()
        {
            myStandardPaperSize = new DCSystem_Drawing.Size[120];
            // 定义标准页面大小
            myStandardPaperSize[(int) PaperKind.A2] = new DCSystem_Drawing.Size(1654, 2339); 	//A2 纸（420 毫米 × 594 毫米）。
            myStandardPaperSize[(int) PaperKind.A3] = new DCSystem_Drawing.Size(1169, 1654); 	//A3 纸（297 毫米 × 420 毫米）。
            myStandardPaperSize[(int) PaperKind.A3Extra] = new DCSystem_Drawing.Size(1268, 1752); 	//A3 extra 纸（322 毫米 × 445 毫米）。
            myStandardPaperSize[(int) PaperKind.A3ExtraTransverse] = new DCSystem_Drawing.Size(1268, 1752); 	//A3 extra transverse 纸（322 毫米 × 445 毫米）。
            myStandardPaperSize[(int) PaperKind.A3Rotated] = new DCSystem_Drawing.Size(1654, 1169); 	//A3 rotated 纸（420 毫米 × 297 毫米）。
            myStandardPaperSize[(int) PaperKind.A3Transverse] = new DCSystem_Drawing.Size(1169, 1654); 	//A3 transverse 纸（297 毫米 × 420 毫米）。
            myStandardPaperSize[(int) PaperKind.A4] = new DCSystem_Drawing.Size(827, 1169); 	//A4 纸（210 毫米 × 297 毫米）。
            myStandardPaperSize[(int) PaperKind.A4Extra] = new DCSystem_Drawing.Size(929, 1268); 	//A4 extra 纸（236 毫米 × 322 毫米）。该值是针对 PostScript 驱动程序的，仅供 Linotronic 打印机使用以节省纸张。
            myStandardPaperSize[(int) PaperKind.A4Plus] = new DCSystem_Drawing.Size(827, 1299); 	//A4 plus 纸（210 毫米 × 330 毫米）。
            myStandardPaperSize[(int) PaperKind.A4Rotated] = new DCSystem_Drawing.Size(1169, 827); 	//A4 rotated 纸（297 毫米 × 210 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.A4Small] = new DCSystem_Drawing.Size(827, 1169); 	//A4 small 纸（210 毫米 × 297 毫米）。
            myStandardPaperSize[(int) PaperKind.A4Transverse] = new DCSystem_Drawing.Size(827, 1169); 	//A4 transverse 纸（210 毫米 × 297 毫米）。
            myStandardPaperSize[(int) PaperKind.A5] = new DCSystem_Drawing.Size(583, 827); 	//A5 纸（148 毫米 × 210 毫米）。
            myStandardPaperSize[(int) PaperKind.A5Extra] = new DCSystem_Drawing.Size(685, 925); 	//A5 extra 纸（174 毫米 × 235 毫米）。
            myStandardPaperSize[(int) PaperKind.A5Rotated] = new DCSystem_Drawing.Size(827, 583); 	//A5 rotated 纸（210 毫米 × 148 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.A5Transverse] = new DCSystem_Drawing.Size(583, 827); 	//A5 transverse 纸（148 毫米 × 210 毫米）。
            myStandardPaperSize[(int) PaperKind.A6] = new DCSystem_Drawing.Size(413, 583); 	//A6 纸（105 毫米 × 148 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.A6Rotated] = new DCSystem_Drawing.Size(583, 413); 	//A6 rotated 纸（148 毫米 × 105 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.APlus] = new DCSystem_Drawing.Size(894, 1402); 	//SuperA/SuperA/A4 纸（227 毫米 × 356 毫米）。
            myStandardPaperSize[(int) PaperKind.B4] = new DCSystem_Drawing.Size(984, 1390); 	//B4 纸（250 × 353 毫米）。
            myStandardPaperSize[(int) PaperKind.B4Envelope] = new DCSystem_Drawing.Size(984, 1390); 	//B4 信封（250 × 353 毫米）。
            myStandardPaperSize[(int) PaperKind.B5] = new DCSystem_Drawing.Size(693, 984); 	//B5 纸（176 毫米 × 250 毫米）。
            myStandardPaperSize[(int) PaperKind.B5Envelope] = new DCSystem_Drawing.Size(693, 984); 	//B5 信封（176 毫米 × 250 毫米）。
            myStandardPaperSize[(int) PaperKind.B5Extra] = new DCSystem_Drawing.Size(791, 1087); 	//ISO B5 extra 纸（201 毫米 × 276 毫米）。
            myStandardPaperSize[(int) PaperKind.B5JisRotated] = new DCSystem_Drawing.Size(1012, 717); 	//JIS B5 rotated 纸（257 毫米 × 182 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.B5Transverse] = new DCSystem_Drawing.Size(717, 1012); 	//JIS B5 transverse 纸（182 毫米 × 257 毫米）。
            myStandardPaperSize[(int) PaperKind.B6Envelope] = new DCSystem_Drawing.Size(693, 492); 	//B6 信封（176 毫米 × 125 毫米）。
            myStandardPaperSize[(int) PaperKind.B6Jis] = new DCSystem_Drawing.Size(504, 717); 	//JIS B6 纸（128 毫米 × 182 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.BPlus] = new DCSystem_Drawing.Size(1201, 1917); 	//SuperB/SuperB/A3 纸（305 毫米 × 487 毫米）。
            myStandardPaperSize[(int) PaperKind.C3Envelope] = new DCSystem_Drawing.Size(1201, 1917); 	//SuperB/SuperB/A3 纸（305 毫米 × 487 毫米）。
            myStandardPaperSize[(int) PaperKind.C4Envelope] = new DCSystem_Drawing.Size(902, 1276); 	//C4 信封（229 毫米 × 324 毫米）。
            myStandardPaperSize[(int) PaperKind.C5Envelope] = new DCSystem_Drawing.Size(638, 902); 	//C5 信封（162 毫米 × 229 毫米）。
            myStandardPaperSize[(int) PaperKind.C65Envelope] = new DCSystem_Drawing.Size(449, 902); 	//C65 信封（114 毫米 × 229 毫米）。
            myStandardPaperSize[(int) PaperKind.C6Envelope] = new DCSystem_Drawing.Size(449, 638); 	//C6 信封（114 毫米 × 162 毫米）。
            myStandardPaperSize[(int) PaperKind.CSheet] = new DCSystem_Drawing.Size(449, 638); 	//C6 信封（114 毫米 × 162 毫米）。 
            //////////myStandardPaperSize[(int) PaperKind.Custom] = new DCSystem_Drawing.Size(0, 0); // 自定义大小
            myStandardPaperSize[(int) PaperKind.DLEnvelope] = new DCSystem_Drawing.Size(433, 866); 	//DL 信封（110 毫米 × 220 毫米）。
            myStandardPaperSize[(int) PaperKind.DSheet] = new DCSystem_Drawing.Size(2201, 3402); 	//D 纸（559 毫米 × 864 毫米）。
            myStandardPaperSize[(int) PaperKind.ESheet] = new DCSystem_Drawing.Size(3402, 4402); 	//E 纸（864 毫米 × 1118 毫米）。
            myStandardPaperSize[(int) PaperKind.Executive] = new DCSystem_Drawing.Size(724, 1051); 	//Executive 纸（184 毫米 × 267 毫米）。
            myStandardPaperSize[(int) PaperKind.Folio] = new DCSystem_Drawing.Size(850, 1299); 	//Folio 纸（216 毫米 × 330 毫米）。
            myStandardPaperSize[(int) PaperKind.GermanLegalFanfold] = new DCSystem_Drawing.Size(850, 1299); 	//German legal fanfold（216 毫米 × 330 毫米）。
            myStandardPaperSize[(int) PaperKind.GermanStandardFanfold] = new DCSystem_Drawing.Size(850, 1201); 	//German standard fanfold（216 毫米 × 305 毫米）。
            myStandardPaperSize[(int) PaperKind.InviteEnvelope] = new DCSystem_Drawing.Size(866, 866); 	//Invite envelope（220 毫米 × 220 毫米）。
            myStandardPaperSize[(int) PaperKind.IsoB4] = new DCSystem_Drawing.Size(984, 1390); 	//ISO B4（250 毫米 × 353 毫米）。
            myStandardPaperSize[(int) PaperKind.ItalyEnvelope] = new DCSystem_Drawing.Size(433, 906); 	//Italy envelope（110 毫米 × 230 毫米）。
            myStandardPaperSize[(int) PaperKind.JapaneseDoublePostcard] = new DCSystem_Drawing.Size(787, 583); 	//Japanese double postcard（200 毫米 × 148 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.JapaneseDoublePostcardRotated] = new DCSystem_Drawing.Size(583, 787); 	//Japanese rotated double postcard（148 毫米 × 200 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.JapanesePostcard] = new DCSystem_Drawing.Size(394, 583); 	//Japanese postcard（100 毫米 × 148 毫米）。
            myStandardPaperSize[(int) PaperKind.JapanesePostcardRotated] = new DCSystem_Drawing.Size(583, 394); 	//Japanese rotated postcard（148 毫米 × 100 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.Ledger] = new DCSystem_Drawing.Size(1701, 1098); 	//Ledger 纸（432 × 279 毫米）。
            myStandardPaperSize[(int) PaperKind.Legal] = new DCSystem_Drawing.Size(850, 1402); 	//Legal 纸（216 × 356 毫米）。
            myStandardPaperSize[(int) PaperKind.LegalExtra] = new DCSystem_Drawing.Size(929, 1500); 	//Legal extra 纸（236 毫米 × 381 毫米）。该值特定于 PostScript 驱动程序，仅供 Linotronic 打印机使用以节省纸张。
            myStandardPaperSize[(int) PaperKind.Letter] = new DCSystem_Drawing.Size(850, 1098); 	//Letter 纸（216 毫米 × 279 毫米）。
            myStandardPaperSize[(int) PaperKind.LetterExtra] = new DCSystem_Drawing.Size(929, 1197); 	//Letter extra 纸（236 毫米 × 304 毫米）。该值特定于 PostScript 驱动程序，仅供 Linotronic 打印机使用以节省纸张。
            myStandardPaperSize[(int) PaperKind.LetterExtraTransverse] = new DCSystem_Drawing.Size(929, 1201); 	//Letter extra transverse 纸（236 毫米 × 305 毫米）。
            myStandardPaperSize[(int) PaperKind.LetterPlus] = new DCSystem_Drawing.Size(850, 1268); 	//Letter plus 纸（216 毫米 毫米 × 322 毫米）。
            myStandardPaperSize[(int) PaperKind.LetterRotated] = new DCSystem_Drawing.Size(1098, 850); 	//Letter rotated 纸（279 毫米 × 216 毫米）。
            myStandardPaperSize[(int) PaperKind.LetterSmall] = new DCSystem_Drawing.Size(850, 1098); 	//Letter small 纸（216 × 279 毫米）。
            myStandardPaperSize[(int) PaperKind.LetterTransverse] = new DCSystem_Drawing.Size(827, 1098); 	//Letter transverse 纸（210 毫米 × 279 毫米）。
            myStandardPaperSize[(int) PaperKind.MonarchEnvelope] = new DCSystem_Drawing.Size(386, 752); 	//Monarch envelope（98 毫米 × 191 毫米）。
            myStandardPaperSize[(int) PaperKind.Note] = new DCSystem_Drawing.Size(850, 1098); 	//Note 纸（216 × 279 毫米）。
            myStandardPaperSize[(int) PaperKind.Number10Envelope] = new DCSystem_Drawing.Size(413, 949); 	//#10 envelope（105 × 241 毫米）。
            myStandardPaperSize[(int) PaperKind.PersonalEnvelope] = new DCSystem_Drawing.Size(362, 650); 	//6 3/4 envelope（92 毫米 × 165 毫米）。
            myStandardPaperSize[(int) PaperKind.Prc16K] = new DCSystem_Drawing.Size(575, 846); 	//PRC 16K 纸（146 × 215 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.Prc16KRotated] = new DCSystem_Drawing.Size(575, 846); 	//PRC 16K rotated 纸（146 × 215 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.Prc32K] = new DCSystem_Drawing.Size(382, 594); 	//PRC 32K 纸（97 × 151 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.Prc32KBig] = new DCSystem_Drawing.Size(382, 594); 	//PRC 32K(Big) 纸（97 × 151 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.Prc32KBigRotated] = new DCSystem_Drawing.Size(382, 594); 	//PRC 32K rotated 纸（97 × 151 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.Prc32KRotated] = new DCSystem_Drawing.Size(382, 594); 	//PRC 32K rotated 纸（97 × 151 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber1] = new DCSystem_Drawing.Size(402, 650); 	//PRC #1 envelope（102 × 165 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber10] = new DCSystem_Drawing.Size(1276, 1803); 	//PRC #10 envelope（324 × 458 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber10Rotated] = new DCSystem_Drawing.Size(1803, 1276); 	//PRC #10 rotated envelope（458 × 324 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber1Rotated] = new DCSystem_Drawing.Size(650, 402); 	//PRC #1 rotated envelope（165 × 102 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber2] = new DCSystem_Drawing.Size(402, 693); 	//PRC #2 envelope（102 × 176 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber2Rotated] = new DCSystem_Drawing.Size(693, 402); 	//PRC #2 rotated envelope（176 × 102 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber3] = new DCSystem_Drawing.Size(492, 693); 	//PRC #3 envelope（125 × 176 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber3Rotated] = new DCSystem_Drawing.Size(693, 492); 	//PRC #3 rotated envelope（176 × 125 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber4] = new DCSystem_Drawing.Size(433, 819); 	//PRC #4 envelope（110 × 208 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber4Rotated] = new DCSystem_Drawing.Size(819, 433); 	//PRC #4 rotated envelope（208 × 110 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber5] = new DCSystem_Drawing.Size(433, 866); 	//PRC #5 envelope（110 × 220 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber5Rotated] = new DCSystem_Drawing.Size(866, 433); 	//PRC #5 rotated envelope（220 × 110 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber6] = new DCSystem_Drawing.Size(472, 906); 	//PRC #6 envelope（120 × 230 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber6Rotated] = new DCSystem_Drawing.Size(906, 472); 	//PRC #6 rotated envelope（230 × 120 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber7] = new DCSystem_Drawing.Size(630, 906); 	//PRC #7 envelope（160 × 230 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber7Rotated] = new DCSystem_Drawing.Size(906, 630); 	//PRC #7 rotated envelope（230 × 160 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber8] = new DCSystem_Drawing.Size(472, 1217); 	//PRC #8 envelope（120 × 309 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber8Rotated] = new DCSystem_Drawing.Size(1217, 472); 	//PRC #8 rotated envelope（309 × 120 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber9] = new DCSystem_Drawing.Size(902, 1276); 	//PRC #9 envelope（229 × 324 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.PrcEnvelopeNumber9Rotated] = new DCSystem_Drawing.Size(902, 1276); 	//PRC #9 rotated envelope（229 × 324 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.Quarto] = new DCSystem_Drawing.Size(846, 1083); 	//Quarto 纸（215 毫米 × 275 毫米）。
            myStandardPaperSize[(int) PaperKind.Standard10x11] = new DCSystem_Drawing.Size(1000, 1098); 	//Standard 纸（254 毫米 × 279 毫米）。
            myStandardPaperSize[(int) PaperKind.Standard10x14] = new DCSystem_Drawing.Size(1000, 1402); 	//Standard 纸（254 毫米 × 356 毫米）。
            myStandardPaperSize[(int) PaperKind.Standard11x17] = new DCSystem_Drawing.Size(1098, 1701); 	//Standard 纸（279 毫米 × 432 毫米）。
            myStandardPaperSize[(int) PaperKind.Standard12x11] = new DCSystem_Drawing.Size(1201, 1098); 	//Standard 纸（305 × 279 毫米）。需要 Windows 98、Windows NT 4.0 或更高版本。
            myStandardPaperSize[(int) PaperKind.Standard15x11] = new DCSystem_Drawing.Size(1500, 1098); 	//Standard 纸（381 毫米 × 279 毫米）。
            myStandardPaperSize[(int) PaperKind.Standard9x11] = new DCSystem_Drawing.Size(902, 1098); 	//Standard 纸（229 × 279 毫米）。
            myStandardPaperSize[(int) PaperKind.Statement] = new DCSystem_Drawing.Size(551, 850); 	//Statement 纸（140 毫米 × 216 毫米）。
            myStandardPaperSize[(int) PaperKind.Tabloid] = new DCSystem_Drawing.Size(1098, 1701); 	//Tabloid 纸（279 毫米 × 432 毫米）。
            myStandardPaperSize[(int) PaperKind.TabloidExtra] = new DCSystem_Drawing.Size(1169, 1799); 	//Tabloid extra 纸（297 毫米 × 457 毫米）。该值特定于 PostScript 驱动程序，仅供 Linotronic 打印机使用以节省纸张。
            myStandardPaperSize[(int) PaperKind.USStandardFanfold] = new DCSystem_Drawing.Size(1488, 1098); 	//US standard fanfold（378 毫米 × 279 毫米）。

        }
    }
}
