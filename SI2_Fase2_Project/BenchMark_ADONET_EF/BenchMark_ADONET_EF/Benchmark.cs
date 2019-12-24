using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace BenchMark_ADONET_EF
{

    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(Job.MediumRun
                    .WithLaunchCount(1)
                    .With(InProcessEmitToolchain.Instance)
                    .WithId("InProcess"));
        }
    }

    [RankColumn]
    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {

        [Benchmark]
        public void BenchCreateClient_EF()
        {

        }

        [Benchmark]
        public void BenchCreateClient_ADONET()
        {

        }
    }
}
