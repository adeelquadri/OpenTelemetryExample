//using System.Diagnostics.Metrics;
//using System.Runtime.CompilerServices;

//namespace OpenTelemetryExample.API.Configs
//{
//    public static class DiagnosticsConfig
//    {
//        public static Meter Meter { get; } = new(AppDomain.CurrentDomain.FriendlyName);

//        public static Counter<int> HitCouter { get; } = Meter.CreateCounter<int>("hits.count", description: "No of times accessed weather Get method");
//        public static Gauge<int> Cold { get; } = Meter.CreateGauge<int>("too.cold");
//        public static Histogram<int> ColdHistory { get; } = Meter.CreateHistogram<int>("temp.history", "c", "History of temps");
//    }
//}
