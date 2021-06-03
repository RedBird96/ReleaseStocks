using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public enum FeatureTypes
    {
        FCFGrowth = 0,
        FCFMedian = 1,
        ROICGrowth = 2,
        ROICMedian = 3,
        EBITDAGrowth = 4,
        EBITDAMedian = 5,
        FIPScore = 6,
        EVGrowth = 7,
        EVMedian = 8,
        BVPSGrowth = 9,
        BVPSMedian = 10,
        MTUM = 11,
        SORTINO_FIP = 12,
        CROICGrowth = 13,
        CROICMedian = 14,
        SALESGrowth = 15,
        SALESMedian = 16,
        ROICScore = 17,
        CROICScore = 18,
        FCFScore = 19,
        EBITDAScore = 20,
        SALESScore = 21,
        VRUniverse = 22,
        VRSector = 23,
        VRHistory = 24,
        SHARPE_FIP = 25,
        PEG_RATIO = 26,
        EBITDA_Liabilities = 27,
        GP_Assets = 28,
        FCF_Sales = 29,
        EV_EBITDA = 30,
        Close_FCF = 31,
        EV_Revenue = 32,
        Dividend_BuyBackRate = 33,
        Operating_Margin = 34,
        ROIC_EV_EBITDA = 35
    }

    public enum FilterTypes
    {
        [Display(Name = "Closing Price")]
        ClosingPrice = 0,
        [Display(Name = "MovingAverage")]
        MovingAverage,
        [Display(Name = "RoCVSBenchmark")]
        RoCVSBenchmark,
        [Display(Name = "RoC")]
        RoC,
        Volume,
        [Display(Name = "Avg Volume")]
        AvgVolume,
        [Display(Name = "Mkt Capitalization")]
        MktCapitalization,
        [Display(Name = "Debt To EBIT")]
        DebtToEBIT,
        [Display(Name = "Positive EBIT years")]
        PositiveEBITYears,
        [Display(Name = "Curr Ratio")]
        CurrentRatio,
        [Display(Name = "Dividend Yield %")]
        ShareholdersYield,
        [Display(Name = "Buy Back Rate")]
        BuyBackRate,
        [Display(Name = "Debt Reduction")]
        DebtReduction,
        [Display(Name = "QoQ Earnings")]
        QoQEarnings,
        [Display(Name = "EV / EBITDA")]
        EV_EBITDA,
        [Display(Name = "ROIC Score")]
        ROICScore,
        [Display(Name = "CROIC Score")]
        CROICScore,
        [Display(Name = "FCFPS Score")]
        FCFScore,
        [Display(Name = "EBITDA Score")]
        EBITDAScore,
        [Display(Name = "SALES Score")]
        SALESScore,
        [Display(Name = "P/E")]
        PE,
        [Display(Name = "P/FCF")]
        PFCF,
        [Display(Name = "P/S")]
        PS,
        [Display(Name = "P/B")]
        PB,
        [Display(Name = "Composite %")]
        Composite,
        [Display(Name = "VR Universe")]
        VRUniverse,
        [Display(Name = "VR Sector")]
        VRSector,
        [Display(Name = "VR History")]
        VRHistory
    }

    public enum TradeType
    {
        [Display(Name = "Buy")]
        Buy = 0,
        [Display(Name = "Add Long")]
        ReBuy = 1,
        [Display(Name = "Cover All")]
        BuyAll = 2,
        [Display(Name = "Cover Partial")]
        BuyPart = 3,
        [Display(Name = "Cover All and Buy")]
        BuyAllAndMore = 4,
        [Display(Name = "Sell")]
        Sell = 5,
        [Display(Name = "Add Short")]
        ReSell = 6,
        [Display(Name = "Exit All")]
        SellAll = 7,
        [Display(Name = "Sell Partial")]
        SellPart = 8,
        [Display(Name = "Exit All and Sell")]
        SellAllAndMore = 9
    }

    public enum SellingType
    {
        Long = 0,
        Short = 1
    }
    public enum FilterRanges
    {
        MoreThan = 0,
        LessThan = 1,
        Between = 2,
        CompareEquation = 3,
    }

    public enum GraphMetrics
    {
        [Display(Name = "Price")]
        Price,
        [Display(Name = "Composite")]
        Composite,
        [Display(Name = "P / E")]
        PE,
        [Display(Name = "P / S")]
        PS,
        [Display(Name = "P / FCF")]
        PFCF,
        [Display(Name = "P / B")]
        PB,
        [Display(Name = "Price To Median PE")]
        PriceToMaxPE
    }

    public enum SystemParameters
    {
        FullDownloadCompleted = 1,
        LastUpdateDate = 2,
        CompositeRollingMedianYears = 3,
        BenchmarksToShow = 4,
        MailConfigFrom = 5,
        MailConfigPort = 6,
        MailConfigSmtp = 7,
        MailConfigUser = 8,
        MailConfigPass = 9
    }

    public enum RiskModelResults
    {
        Normal = 0,
        NoNewPositions = 1,
        SellSPY = 2,
        SellAllBuySPXU = 3
    }

    public enum StockTypes
    {
        Regular = 0,
        FileBased = 1,
        Benchmark = 2,
        IExtradingBenchmark = 3
    }

    public enum RiskMetrics
    {
        Volatility,
        VolatilityAnnualized,
        DownsideDeviationMonthly,
        DD1,
        DD2,
        DD3,
        BenchmarkCorrelation,
        BetaLifetime,
        RollingBeta3Years,
        AlphaAnnualized,
        AvgAnnualAlpha,
        R2,
        SharpeRatio,
        SortinoRatio,
        TreynorRatio,
        Calmar,
        InformationRatio,
        UpsideCapture,
        DownsideCapture,
        ProfitFactor,
        PercPositiveMonths,
        PercNegativeMonths,
        WinStreak,
        LoseStreak,
        DD1Plain,
        VARHistorical
    }

    public enum Indicators
    {
        [Display(Name = "Volume")]
        volume,
        [Display(Name = "Close Price")]
        closeprice,
        [Display(Name = "Earnings Before Interest Taxes & Depreciation Amortization (EBITDA)")]
        ebitda,
        [Display(Name = "Earnings Before Interest Taxes & Depreciation Amortization (USD)")]
        ebitdausd,
        [Display(Name = "EBITDA Margin")]
        ebitdamargin,
        [Display(Name = "Shares (Basic)")]
        sharesbas,
        [Display(Name = "Total Debt")]
        debt,
        //[Display(Name = "Total Debt (USD)")]
        //debtusd,
        [Display(Name = "Free Cash Flow")]
        fcf,
        [Display(Name = "Return on Invested Capital")]
        roic,
        [Display(Name = "Current Ratio")]
        currentratio,
        [Display(Name = "Dividend Yield")]
        divyield,
        [Display(Name = "Enterprise Value - Daily")]
        ev,
        [Display(Name = "Book Value per Share")]
        bvps,
        [Display(Name = "Market Capitalization - Daily")]
        marketcap,
        [Display(Name = "Invested Capital")]
        invcap,
        [Display(Name = "Revenues (USD)")]
        revenueusd,
        [Display(Name = "Earnings per Basic Share")]
        eps,
        [Display(Name = "Price to Earnings Ratio")]
        pe1,
        [Display(Name = "Price to Sales Ratio")]
        ps1,
        [Display(Name = "Price to Book Value")]
        pb,
        [Display(Name = "Share Price (Adjusted Close)")]
        price,
        [Display(Name = "Sales per Share")]
        sps,
        [Display(Name = "Free Cash Flow per Share")]
        fcfps,
        [Display(Name = "Cost of Revenue")]
        cor,
        [Display(Name = "Net Income")]
        netinc,
        [Display(Name = "Earnings per Diluted Share")]
        epsdil,
        [Display(Name = "Weighted Average Shares")]
        shareswa,
        [Display(Name = "Capital Expenditure")]
        capex,
        [Display(Name = "Total Assets")]
        assets,
        [Display(Name = "Cash and Equivalents")]
        cashneq,
        [Display(Name = "Total Liabilities")]
        liabilities,
        [Display(Name = "Current Assets")]
        assetsc,
        [Display(Name = "Current Liabilities")]
        liabilitiesc,
        [Display(Name = "Tangible Asset Value")]
        tangibles,
        [Display(Name = "Return on Average Equity")]
        roe,
        [Display(Name = "Return on Average Assets")]
        roa,
        [Display(Name = "Gross Profit")]
        gp,
        [Display(Name = "Gross Margin")]
        grossmargin,
        [Display(Name = "Profit Margin")]
        netmargin,
        [Display(Name = "Return on Sales")]
        ros,
        [Display(Name = "Asset Turnover")]
        assetturnover,
        [Display(Name = "Payout Ratio")]
        payoutratio,
        [Display(Name = "Working Capital")]
        workingcapital,
        [Display(Name = "Tangible Assets Book Value per Share")]
        tbvps
    }
}

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetName();
    }
}