using System;
using System.Reflection;
using System.Windows.Markup;
using System.ComponentModel;

namespace ClashEngine.NET.Graphics.Gui
{
	using System.Xaml;
	using Interfaces.Data;
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Binduje wartość wewnątrz kontenera Xaml.
	/// </summary>
	[MarkupExtensionReturnType(typeof(object))]
	public class BindingExtension
		: MarkupExtension, IBindingExtension
	{
		#region Private fields
		private string PropertyName = string.Empty;
		#endregion
		
		#region IBindingExtension Members
		/// <summary>
		/// Tryb bindowania.
		/// Po utworzeniu bindingu zmiana wartości nie odniesie skutku.
		/// </summary>
		public BindingMode Mode { get; set; }

		/// <summary>
		/// Ścieżka do elementu docelowego.
		/// Obsługiwany format: idKontrolki.właściwość/pole.
		/// Po utworzeniu bindingu zmiana wartości nie odniesie skutku.
		/// </summary>
		public string Path { get; set; }
		#endregion

		#region IBinding Members
		public object Source { get; private set; }
		public MemberInfo SourceProperty { get; private set; }
		public object Target { get; private set; }
		public MemberInfo TargetProperty { get; private set; }
		#endregion

		#region MarkupExtension Members
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			//Pobieramy źródło.
			var rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			if (rootProvider == null)
			{
				throw new InvalidOperationException("IRootObjectProvider");
			}
			var rootObject = rootProvider.RootObject as XamlGuiContainer;
			if (rootObject == null)
			{
				throw new InvalidOperationException("RootObject");
			}

			//Pobieramy właściwość, do której mamy przypisywać wartości.
			var targetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			if (targetProvider == null)
			{
				throw new InvalidOperationException("IProvideValueTarget");
			}

			this.Target = targetProvider.TargetObject;
			this.TargetProperty = targetProvider.TargetProperty as PropertyInfo;

			//Pobieramy źródło
			string[] p = this.Path.Split('.');
			if (p.Length != 2)
			{
				throw new InvalidOperationException("Invalid Path format");
			}
			this.PropertyName = p[1].Trim();

			this.Source = rootObject.Controls[p[0].Trim()];
			this.SourceProperty = this.Source.GetType().GetProperty(this.PropertyName);
			if (this.SourceProperty == null)
			{
				this.SourceProperty = this.Source.GetType().GetField(p[1].Trim());
			}

			if (this.Source == null || this.SourceProperty == null)
			{
				throw new InvalidOperationException("Cannot find source");
			}

			//Metoda lokalna, by nie dublować kodu ;)
			Action bindSource = () =>
			{
				if (!(this.Source is INotifyPropertyChanged))
				{
					throw new InvalidOperationException("Source must implement INotifyPropertyChanged for this mode");
				}
				(this.Source as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(SourceToTarget);
			};

			//Zależnie od trybu wiązania dodajemy odpowiednie zdarzenia.
			switch (this.Mode)
			{
			case BindingMode.OneWay:
				bindSource();
				break;
			case BindingMode.TwoWay:
				bindSource();
				if (!(this.Target is INotifyPropertyChanged))
				{
					throw new InvalidOperationException("Target must implement INotifyPropertyChanged for this mode");
				}
				(this.Target as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(TargetToSource);
				break;
			}

			//Dla wszystkich trybów musimy zwrócić aktualną wartość.
			if (this.SourceProperty is PropertyInfo)
			{
				return (this.SourceProperty as PropertyInfo).GetValue(this.Source, null);
			}
			return (this.SourceProperty as FieldInfo).GetValue(this.Source);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowe wiązanie.
		/// </summary>
		public BindingExtension()
		{
			this.Mode = BindingMode.OneWay;
		}

		/// <summary>
		/// Inicjalizuje nowe wiązanie.
		/// </summary>
		/// <param name="path">Ścieżka do elementu.</param>
		public BindingExtension(string path)
		{
			this.Mode = BindingMode.OneWay;
			this.Path = path;
		}
		#endregion

		#region Private binding methods
		/// <summary>
		/// Źródło do celu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SourceToTarget(object sender, PropertyChangedEventArgs e)
		{
			if (this.SourceProperty is PropertyInfo)
			{
				(this.TargetProperty as PropertyInfo).SetValue(this.Target,
					(this.SourceProperty as PropertyInfo).GetValue(this.Source, null), null);
			}
			else
			{
				(this.TargetProperty as PropertyInfo).SetValue(this.Target,
					(this.SourceProperty as FieldInfo).GetValue(this.Source), null);
			}
		}


		/// <summary>
		/// Cel do źródła.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TargetToSource(object sender, PropertyChangedEventArgs e)
		{
			if (this.SourceProperty is PropertyInfo)
			{
				(this.SourceProperty as PropertyInfo).SetValue(this.Source, 
					(this.TargetProperty as PropertyInfo).GetValue(this.Target, null), null);
			}
			else
			{
				(this.SourceProperty as FieldInfo).SetValue(this.Source,
					(this.TargetProperty as PropertyInfo).GetValue(this.Target, null));
			}
		}		
		#endregion
	}
}
