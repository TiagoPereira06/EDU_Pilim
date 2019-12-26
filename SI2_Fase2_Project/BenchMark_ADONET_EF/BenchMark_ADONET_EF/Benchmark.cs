using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Si2_Fase2_EF;
using System;

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
        public void ExercicioF_EF()
        {
            Exercicios_EF.ExercicioF("IE00BXC8D038", 420, new DateTime(2019, 12, 26, 16, 0, 0));
        }

        [Benchmark]
        public void RemoveData_ExercicioF_EF()
        {
            using(var ctx = new PilimEntities())
            {
                Triplos triplo = ctx.Triplos.Find("IE00BXC8D038", new DateTime(2019, 12, 26, 16, 0, 0));
                ctx.Triplos.Attach(triplo);
                ctx.Triplos.Remove(triplo);
            }
        }

        [Benchmark]
        public void ExercicioF_ADONET()
        {

        }
    }
}
