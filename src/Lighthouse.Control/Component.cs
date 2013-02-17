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
		string ComponentName { get; }
		byte ComponentAddress { get; }
		event EventHandler<PropertyChangedEventArgs> PropertyChanged;
		event EventHandler<PropertyChangedEventArgs> PropertySet;

		void UpdateProperty(byte propertyReference, byte value);

		/// <summary>
		/// this returns any property updates that still are to be flushed to the device
		/// </summary>
		/// <param name="propertyReference"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		IDictionary<byte, byte> UpdatedProperties();
	}

	public abstract class BaseComponent : IComponent
	{
		string _componentName;
		byte _componentAddress;

		ConcurrentDictionary<byte, byte> localProperties = new ConcurrentDictionary<byte, byte>();
		ConcurrentDictionary<byte, byte> deviceProperties = new ConcurrentDictionary<byte, byte>();

		byte IComponent.ComponentAddress
		{
			get { return _componentAddress; }
		}

		public string ComponentName
		{
			get { return _componentName; }
		}
		public EventHandler<PropertyChangedEventArgs> PropertyChanged;

		event EventHandler<PropertyChangedEventArgs> IComponent.PropertyChanged
		{
			add { PropertyChanged += value; }
			remove { PropertyChanged -= value; }
		}


		public EventHandler<PropertyChangedEventArgs> PropertySet;

		event EventHandler<PropertyChangedEventArgs> IComponent.PropertySet
		{
			add { PropertySet += value; }
			remove { PropertySet -= value; }
		}

		public BaseComponent(byte componentAddress, string componentName)
		{
			_componentName = componentName;
			_componentAddress = componentAddress;
		}

		protected byte Get(byte prop)
		{
			byte tmp = 0;
			if (localProperties.TryGetValue(prop, out tmp))
			{
				return tmp;
			}
			else if (deviceProperties.TryGetValue(prop, out tmp))
			{
				return tmp;
			}
			else
			{
				return default(byte);
			}
				
		}

		protected void Set(byte prop, byte val)
		{

			byte tmp;

			if (deviceProperties.TryGetValue(prop, out tmp) && tmp == val)
			{
				localProperties.TryRemove(prop, out tmp);
				return;
			}
			if (!localProperties.TryGetValue(prop, out tmp) || tmp != val)
			{
				localProperties.AddOrUpdate(prop, val, (c, d) =>
				{
					return val;
				});
				if (PropertySet != null)
					PropertySet.Invoke(this, new PropertyChangedEventArgs(prop, val));
				if (PropertyChanged != null)
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop, val));
			}
		}

		void IComponent.UpdateProperty(byte prop, byte val)
		{
			byte tmp;
			localProperties.TryRemove(prop, out tmp);

			if (!deviceProperties.TryGetValue(prop, out tmp) || tmp != val)
			{
				deviceProperties.AddOrUpdate(prop, val, (c, d) => val);
				if (PropertyChanged != null)
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop, val));
			}
		}

		IDictionary<byte, byte> IComponent.UpdatedProperties()
		{
			Dictionary<byte, byte> buffer = new Dictionary<byte, byte>();
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
		public byte Property { get; private set; }
		public byte Value { get; private set; }
		public PropertyChangedEventArgs(byte property, byte value)
		{
			Property = property;
			Value = value;
		}
	}
}
