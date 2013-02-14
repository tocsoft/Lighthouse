using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public interface IComponent 
	{
		/// <summary>
		/// this hardware name for this component
		/// </summary>
		string ComponentAddress { get; }
		event EventHandler<PropertyChangedEventArgs> PropertyChanged;

		void UpdateProperty(string propertyReference, byte value);

		/// <summary>
		/// this returns any property updates that still are to be flushed to the device
		/// </summary>
		/// <param name="propertyReference"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		IDictionary<string, byte> UpdatedProperties();
	}

	public abstract class BaseComponent : IComponent
	{
		string _componentAddress;

		ConcurrentDictionary<string, byte> localProperties = new ConcurrentDictionary<string, byte>();
		ConcurrentDictionary<string, byte> deviceProperties = new ConcurrentDictionary<string, byte>();

		string IComponent.ComponentAddress
		{
			get { return _componentAddress; }
		}
		public EventHandler<PropertyChangedEventArgs> PropertyChanged;

		event EventHandler<PropertyChangedEventArgs> IComponent.PropertyChanged
		{
			add { PropertyChanged += value; }
			remove { PropertyChanged -= value; }
		}

		public BaseComponent(string componentAddress) {

			_componentAddress = componentAddress;
		}

		protected byte Get(string prop){

			if (localProperties.ContainsKey(prop))
			{
				return localProperties[prop];
			}
			else if (deviceProperties.ContainsKey(prop))
			{
				return deviceProperties[prop];
			}
			else {
				return default(byte);
			}
				
		}

		protected void Set(string prop, byte val)
		{

			byte tmp;
			if (deviceProperties.ContainsKey(prop))
			{
				if (deviceProperties[prop] == val)
				{
					localProperties.TryRemove(prop, out tmp);
					return;
				}
			}

			if (!localProperties.ContainsKey(prop) || localProperties[prop] != val)
			{
				localProperties.AddOrUpdate(prop, val, (c, d) =>
				{
					return val;
				});
				if (PropertyChanged != null)
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop, val));
			}


		}

		void IComponent.UpdateProperty(string propertyReference, byte value)
		{
			byte tmp;
			localProperties.TryRemove(propertyReference, out tmp);
			deviceProperties.AddOrUpdate(propertyReference, value, (c, d) => value);
		}




		IDictionary<string, byte> IComponent.UpdatedProperties()
		{
			Dictionary<string, byte> buffer = new Dictionary<string,byte>();
			byte tmp;
			foreach (var prop in localProperties) {
				deviceProperties.AddOrUpdate(prop.Key, prop.Value, (c, d) => prop.Value);
				buffer.Add(prop.Key, prop.Value);
				localProperties.TryRemove(prop.Key, out  tmp);
			}

			return buffer;
		}


		
	}

	public class PropertyChangedEventArgs : EventArgs
	{
		public string Property { get; private set; }
		public byte Value { get; private set; }
		public PropertyChangedEventArgs(string property, byte value) {
			Property = property;
			Value = value;
		}
	}
}
