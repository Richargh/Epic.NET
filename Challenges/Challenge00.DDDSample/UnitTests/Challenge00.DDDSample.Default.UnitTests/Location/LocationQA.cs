//  
//  LocationTester.cs
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
using NUnit.Framework;
using System;
using Challenge00.DDDSample.Location;
using Rhino.Mocks;
namespace DefaultImplementation.Location
{
	[TestFixture()]
	public class LocationQA
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_01 ()
		{
			// arrange:
			
		
			// act:
			new Challenge00.DDDSample.Location.Location(null, "Name");
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_02 ()
		{
			// arrange:
			UnLocode code = new UnLocode("UNLOC");
		
			// act:
			new Challenge00.DDDSample.Location.Location(code, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_03 ()
		{
			// arrange:
			UnLocode code = new UnLocode("UNLOC");
		
			// act:
			new Challenge00.DDDSample.Location.Location(code, string.Empty);
		}
		
		[Test]
		public void Ctor_04()
		{
			// arrange:
			UnLocode code = new UnLocode("UNLOC");
			string name = "Test Location";
		
			// act:
			ILocation location = new Challenge00.DDDSample.Location.Location(code, name);
		
			// assert:
			Assert.AreEqual(code, location.UnLocode);
			Assert.AreEqual(name, location.Name);
		}
	}
}

