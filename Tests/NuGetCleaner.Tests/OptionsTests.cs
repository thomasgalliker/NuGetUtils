// Copyright (c) 2019 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;
using System.IO;
using CommandLine;
using NUnit.Framework;

namespace SIL.NuGetCleaner.Tests
{
	[TestFixture]
	public class OptionsTests
	{
		private Options _sut;

		[SetUp]
		public void SetUp()
		{
			_sut = null;
		}

		[Test]
		public void HappyPath()
		{
			Parser.Default.ParseArguments<Options>(new[] { "--api-key=12345", "mypackage" })
				.WithParsed(options => _sut = options)
				.WithNotParsed(err => throw new Exception());
			Assert.That(_sut.ApiKey, Is.EqualTo("12345"));
			Assert.That(_sut.PackageId, Is.EqualTo("mypackage"));
		}

		[Test]
		public void HappyPathReverse()
		{
			Parser.Default.ParseArguments<Options>(new[] { "mypackage", "--api-key=12345" })
				.WithParsed(options => _sut = options)
				.WithNotParsed(err => throw new Exception());
			Assert.That(_sut.ApiKey, Is.EqualTo("12345"));
			Assert.That(_sut.PackageId, Is.EqualTo("mypackage"));
		}

		[Test]
		public void NoArgs_Fails()
		{
			Assert.That(() => Parser.Default.ParseArguments<Options>(new string[] { })
				.WithParsed(options => _sut = options)
				.WithNotParsed(err => throw new Exception()), Throws.Exception);
		}

		[Test]
		public void MissingArg_Fails()
		{
			Assert.That(() => Parser.Default.ParseArguments<Options>(new [] { "--api-key=48" })
				.WithParsed(options => _sut = options)
				.WithNotParsed(err => throw new Exception()), Throws.Exception);
		}

		[Test]
		public void MissingOptionalArg_Ok()
		{
			Assert.That(() => Parser.Default.ParseArguments<Options>(new [] { "foo" })
				.WithParsed(options => _sut = options)
				.WithNotParsed(err => throw new Exception()), Throws.Nothing);
			Assert.That(_sut.PackageId, Is.EqualTo("foo"));
		}
	}
}
