﻿using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Retia.Mathematics;
using Retia.Tests.Plumbing;
using Retia.Training.Data;
using Xunit;
using XunitShould;

namespace Retia.Tests.Training
{
	public class LinearDataSetTests
	{
		[Fact]
		public void CanSaveAndLoadTrainingSet()
		{
			using (var file = new DisposableFile())
			{
				var set = GetTrainingSet();
				var loaded = file.WriteAndReadData(set, LinearDataSet<float>.Load);

				CheckTestSetsEqual(loaded, set);
			}
		}

		[Fact]
		public void CanClone()
		{
			var set = GetTrainingSet();
			var clone = set.Clone();

			CheckTestSetsEqual(clone, set);
		}

		[Fact]
		public void CanResetTrainingSet()
		{
			var set = GetTrainingSet();

			var next = set.GetNextSample();
			set.Samples[0].EqualsTo(next).ShouldBeTrue();

			next = set.GetNextSample();
			set.Samples[1].EqualsTo(next).ShouldBeTrue();

			set.Reset();
			next = set.GetNextSample();
			set.Samples[0].EqualsTo(next).ShouldBeTrue();
		}

		private void CheckTestSetsEqual(LinearDataSet<float> loaded, LinearDataSet<float> set)
		{
			loaded.ShouldNotBeNull();
			loaded.SampleCount.ShouldEqual(set.Samples.Count);
			ReferenceEquals(loaded.Samples, set.Samples).ShouldBeFalse();

			for (int i = 0; i < set.Samples.Count; i++)
			{
				var next = loaded.GetNextSample();
				set.Samples[i].EqualsTo(next).ShouldBeTrue();

				loaded.Samples[i].EqualsTo(set.Samples[i]).ShouldBeTrue();
			}
		}

		private LinearDataSet<float> GetTrainingSet()
		{
			var date = DateTime.UtcNow;

			return
				new LinearDataSet<float>(Enumerable.Range(0, 10).Select(x => new Sample<float>(Matrix<float>.Build.Random(10, 10), Matrix<float>.Build.Random(10, 10))).ToList());
		}
	}
}