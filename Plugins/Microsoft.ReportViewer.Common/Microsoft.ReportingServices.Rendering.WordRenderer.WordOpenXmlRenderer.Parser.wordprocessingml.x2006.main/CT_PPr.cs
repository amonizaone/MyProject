using System.Collections.Generic;
using System.IO;

namespace Microsoft.ReportingServices.Rendering.WordRenderer.WordOpenXmlRenderer.Parser.wordprocessingml.x2006.main
{
	internal class CT_PPr : CT_PPrBase, IOoxmlComplexType
	{
		private CT_SectPr _sectPr;

		public CT_SectPr SectPr
		{
			get
			{
				return _sectPr;
			}
			set
			{
				_sectPr = value;
			}
		}

		public static string SectPrElementName => "sectPr";

		protected override void InitAttributes()
		{
		}

		protected override void InitElements()
		{
		}

		protected override void InitCollections()
		{
		}

		public override void Write(TextWriter s, string tagName)
		{
			WriteOpenTag(s, tagName, null);
			WriteElements(s);
			WriteCloseTag(s, tagName);
		}

		public override void WriteOpenTag(TextWriter s, string tagName, Dictionary<string, string> namespaces)
		{
			WriteOpenTag(s, tagName, "w", namespaces);
		}

		public override void WriteCloseTag(TextWriter s, string tagName)
		{
			s.Write("</w:");
			s.Write(tagName);
			s.Write(">");
		}

		public override void WriteAttributes(TextWriter s)
		{
			base.WriteAttributes(s);
		}

		public override void WriteElements(TextWriter s)
		{
			base.WriteElements(s);
			Write_sectPr(s);
		}

		public void Write_sectPr(TextWriter s)
		{
			if (_sectPr != null)
			{
				_sectPr.Write(s, "sectPr");
			}
		}
	}
}
