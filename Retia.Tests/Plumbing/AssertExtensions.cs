﻿using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Retia.Mathematics;
using Xunit;
using XunitShould;

namespace Retia.Tests.Plumbing
{
    public static class AssertExtensions
    {
        public static void ShouldMatrixEqualWithinError<T>(this Matrix<T> a, Matrix<T> b) where T : struct, IEquatable<T>, IFormattable
        {
            a.AsColumnMajorArray().ShouldArrayEqualWithinError(b.AsColumnMajorArray());
        }

        public static void ShouldArrayEqualWithinError<T>(this T[] array1, T[] array2) where T : struct, IEquatable<T>, IFormattable
        {
            if (array1 == null && array2 == null)
            {
                return;
            }

            array1.ShouldNotBeNull();
            array2.ShouldNotBeNull();

            for (int i = 0; i < array1.Length; i++)
            {
                array1[i].ShouldEqualWithinError(array2[i]);
            }
        }

        public static void ShouldEqualWithinError<T>(this T val, T expected, float error = 1e-5f) where T : struct, IEquatable<T>, IFormattable
        {
            MathProvider<T>.Instance.AlmostEqual(val, expected, error).ShouldBeTrue();
        }

        public static void ShouldHaveSize<T>(this Matrix<T> matrix, int rows, int cols) where T : struct, IEquatable<T>, IFormattable
        {
            matrix.RowCount.ShouldEqual(rows);
            matrix.ColumnCount.ShouldEqual(cols);
        }
    }
}