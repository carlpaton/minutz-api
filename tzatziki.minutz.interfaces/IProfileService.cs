﻿
using System.Collections.Generic;
using System.Security.Claims;
using tzatziki.minutz.models;
using tzatziki.minutz.models.Auth;

namespace tzatziki.minutz.interfaces
{
	public interface IProfileService
	{
		UserProfile GetFromClaims(IEnumerable<Claim> claims, ITokenStringHelper tokenStringHelper, AppSettings appsettings);
	}
}
