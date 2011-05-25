//  
//  EnterpriseBase.cs
//  
//  Author:
//       Giacomo Tesio <giacomo@tesio.it>
// 
//  Copyright (c) 2010-2011 Giacomo Tesio
// 
//  This file is part of Epic.NET.
// 
//  Epic.NET is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Epic.NET is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
// 
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  
using System;
using System.Security.Principal;
using System.Runtime.Serialization;
using Epic;
namespace Epic.Enterprise
{
	/// <summary>
	/// Base class for <see cref="IEnterprise"/> implementations.
	/// </summary>
	/// <exception cref='ArgumentNullException'>
	/// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
	/// </exception>
	/// <exception cref='ArgumentException'>
	/// Is thrown when an argument passed to a method is invalid.
	/// </exception>
	/// <exception cref='InvalidOperationException'>
	/// Is thrown when an operation cannot be performed.
	/// </exception>
	[Serializable]
	public abstract class EnterpriseBase : IEnterprise, ISerializable
	{
		private readonly string _name;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Epic.Enterprise.EnterpriseBase"/> class.
		/// </summary>
		/// <param name='name'>
		/// Name of the enterprise.
		/// </param>
		/// <exception cref='ArgumentNullException'>
		/// Is thrown when <paramref name="name"/> is <see langword="null" /> .
		/// </exception>
		protected EnterpriseBase (string name)
		{
			if(string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			_name = name;
		}
		
		#region Templates methods
		
		/// <summary>
		/// Starts a new working session.
		/// </summary>
		/// <param name='owner'>
		/// Owner of the new session. Will never be <value>null</value>.
		/// </param>
		/// <param name='workingSession'>
		/// The new working session.
		/// </param>
		/// <exception cref="InvalidOperationException"><paramref name="owner"/> can not create a 
		/// new <see cref="IWorkingSession"/>.</exception>
		protected abstract void StartWorkingSession(IPrincipal owner, out WorkingSessionBase workingSession);
		
		/// <summary>
		/// Acquires the working session.
		/// </summary>
		/// <returns>
		/// The working session.
		/// </returns>
		/// <param name='owner'>
		/// The owner. Will never be <value>null</value>.
		/// </param>
		/// <param name='identifier'>
		/// The working session identifier. Will never be <value>null</value> or empty.
		/// </param>
		/// <exception cref="InvalidOperationException"><paramref name="owner"/> can not acquire 
		/// the <see cref="IWorkingSession"/> identified by <paramref name="identifier"/>.</exception>
		protected abstract WorkingSessionBase AcquireWorkingSessionReal (IPrincipal owner, string identifier);

		/// <summary>
		/// Called before that <paramref name="workingSession"/> has been disposed.
		/// </summary>
		/// <param name='owner'>
		/// The owner.
		/// </param>
		/// <param name='workingSession'>
		/// The working session to end.
		/// </param>
		/// <exception cref="InvalidOperationException"><paramref name="owner"/> can not end 
		/// <paramref name="workingSession"/>.</exception>
		protected abstract void BeforeWorkingSessionEnd (IPrincipal owner, WorkingSessionBase workingSession);
				
		#endregion Templates methods
		
		#region IEnterprise implementation
		
		/// <summary>
		/// Name of the enterprise.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name 
		{
			get 
			{
				return _name;
			}
		}
		
		/// <summary>
		/// Starts a new working session. Template method.
		/// </summary>
		/// <param name='owner'>
		/// Owner of the new session.
		/// </param>
		/// <param name='workingSession'>
		/// The new working session.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="owner"/> is <value>null</value>.</exception>
		/// <exception cref="InvalidOperationException"><paramref name="owner"/> can not create a 
		/// new <see cref="IWorkingSession"/>.</exception>
		/// <seealso cref="EnterpriseBase.StartWorkingSession(IPrincipal, WorkingSessionBase)"/>
		public void StartWorkingSession (IPrincipal owner, out IWorkingSession workingSession)
		{
			if(null == owner)
				throw new ArgumentNullException("owner");
			WorkingSessionBase newSession = null;
			StartWorkingSession(owner, out newSession);
			workingSession = newSession;
		}
		
		/// <summary>
		/// Acquires an existing working session. Template method.
		/// </summary>
		/// <returns>
		/// The working session.
		/// </returns>
		/// <param name='owner'>
		/// The owner.
		/// </param>
		/// <param name='identifier'>
		/// The working session identifier.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="owner"/> is <value>null</value>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="identifier"/> is <value>null</value> 
		/// or empty.</exception>
		/// <exception cref="InvalidOperationException"><paramref name="owner"/> can not acquire 
		/// the <see cref="IWorkingSession"/> identified by <paramref name="identifier"/>.</exception>
		/// <seealso cref="EnterpriseBase.AcquireWorkingSessionReal(IPrincipal, string)"/>
		public IWorkingSession AcquireWorkingSession (IPrincipal owner, string identifier)
		{
			if(null == owner)
				throw new ArgumentNullException("owner");
			if(string.IsNullOrEmpty(identifier))
				throw new ArgumentNullException("identifier");
			
			return AcquireWorkingSessionReal (owner, identifier);
		}
		
		/// <summary>
		/// Ends the working session. Template method.
		/// </summary>
		/// <param name='owner'>
		/// The owner.
		/// </param>
		/// <param name='workingSession'>
		/// The working session to end.
		/// </param>
		/// <exception cref="ArgumentNullException">Either <paramref name="owner"/> or 
		/// <paramref name="workingSession"/> are <value>null</value>.</exception>
		/// <exception cref="InvalidOperationException"><paramref name="owner"/> can not end 
		/// <paramref name="workingSession"/>.</exception>
		public void EndWorkingSession (IPrincipal owner, IWorkingSession workingSession)
		{
			if(null == owner)
				throw new ArgumentNullException("owner");
			if(null == workingSession)
				throw new ArgumentNullException("workingSession");
			WorkingSessionBase session = workingSession as WorkingSessionBase;
			if(null == session)
			{
				ArgumentException inner = new ArgumentException("The working session provided do not extend WorkingSessionBase.", "workingSession");
				throw new InvalidOperationException("Unknown working session.", inner);
			}
			try
			{
				BeforeWorkingSessionEnd(owner, session);
				
				session.Dispose();
			}
			catch(Exception e)
			{
				if(e is InvalidOperationException)
					throw e;
				string message = string.Format("The owner ({0}) can not end the working session {1}.", owner.Identity.Name, workingSession.Identifier);
				throw new InvalidOperationException(message, e);
			}
		}

		#endregion

		#region ISerializable implementation
		void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
		{
			info.SetType(typeof(EnterpriseSerializationHelper));
		}
		#endregion
	}
}

