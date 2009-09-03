/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bj�rnar Henden
 * Copyright (C) 2006-2009 Jaben Cargman
 * http://www.yetanotherforum.net/
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using YAF.Classes;
using YAF.Classes.Utils;
using YAF.Classes.Data;

namespace YAF.Pages // YAF.Pages
{
	/// <summary>
	/// Summary description for login.
	/// </summary>
	public partial class login : YAF.Classes.Core.ForumPage
	{

		public login()
			: base( "LOGIN" )
		{
			
		}

		protected void Page_Load( object sender, System.EventArgs e )
		{
			if ( !IsPostBack )
			{
				Login1.MembershipProvider = Config.MembershipProvider;

				PageLinks.AddLink( PageContext.BoardSettings.Name, YafBuildLink.GetLink( ForumPages.forum ) );
				PageLinks.AddLink( GetText( "title" ) );

				//Login1.CreateUserText = "Sign up for a new account.";
				//Login1.CreateUserUrl = YafBuildLink.GetLink( ForumPages.register );
				Login1.PasswordRecoveryText = GetText( "lostpassword" );
				Login1.PasswordRecoveryUrl = YafBuildLink.GetLink( ForumPages.recoverpassword );
				Login1.FailureText = GetText( "password_error" );

				if ( !String.IsNullOrEmpty( Request.QueryString ["ReturnUrl"] ) )
				{
					Login1.DestinationPageUrl = Server.UrlDecode( Request.QueryString ["ReturnUrl"] );
				}
				else
				{
					Login1.DestinationPageUrl = YafBuildLink.GetLink( ForumPages.forum );
				}			

				// localize controls
				CheckBox rememberMe = ControlHelper.FindControlAs<CheckBox>( Login1, "RememberMe" );
				TextBox userName = ControlHelper.FindControlAs<TextBox>( Login1, "UserName" );
				TextBox password = ControlHelper.FindControlAs<TextBox>( Login1, "Password" );
				Button forumLogin = ControlHelper.FindControlAs<Button>( Login1, "LoginButton" );
				Button passwordRecovery = ControlHelper.FindControlAs<Button>( Login1, "PasswordRecovery" );

				/*
				RequiredFieldValidator usernameRequired = ( RequiredFieldValidator ) Login1.FindControl( "UsernameRequired" );
				RequiredFieldValidator passwordRequired = ( RequiredFieldValidator ) Login1.FindControl( "PasswordRequired" );

				usernameRequired.ToolTip = usernameRequired.ErrorMessage = GetText( "REGISTER", "NEED_USERNAME" );
				passwordRequired.ToolTip = passwordRequired.ErrorMessage = GetText( "REGISTER", "NEED_PASSWORD" );
				*/

				if ( rememberMe != null ) rememberMe.Text = GetText( "auto" );
				if ( forumLogin != null ) forumLogin.Text = GetText( "forum_login" );
				if ( passwordRecovery != null )
				{
					passwordRecovery.Text = GetText( "lostpassword" );
				}

				if ( password != null && forumLogin != null)
				{
					password.Attributes.Add( "onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + forumLogin.ClientID + "').click();return false;}} else {return true}; " );
				}

				DataBind();
			}
		}

		protected void Login1_Authenticate( object sender, AuthenticateEventArgs e )
		{
			TextBox userName = ControlHelper.FindControlAs<TextBox>( Login1, "UserName" );
			TextBox password = ControlHelper.FindControlAs<TextBox>( Login1, "Password" );

			e.Authenticated = PageContext.CurrentMembership.ValidateUser( userName.Text.Trim(), password.Text.Trim() );
		}

		protected void Login1_LoginError( object sender, EventArgs e )
		{
			bool emptyFields = false;

			TextBox userName = ControlHelper.FindControlAs<TextBox>( Login1, "UserName" );
			TextBox password = ControlHelper.FindControlAs<TextBox>( Login1, "Password" );

			if ( userName.Text.Trim().Length == 0 )
			{
				PageContext.AddLoadMessage( GetText( "REGISTER", "NEED_USERNAME" ) );
				emptyFields = true;
			}
			if ( password.Text.Trim().Length == 0 )
			{
				PageContext.AddLoadMessage( GetText( "REGISTER", "NEED_PASSWORD" ) );
				emptyFields = true;
			}

			if (!emptyFields) PageContext.AddLoadMessage( Login1.FailureText );
		}

		public override bool IsProtected
		{
			get { return false; }
		}

		protected void PasswordRecovery_Click( object sender, EventArgs e )
		{
			YafBuildLink.Redirect( ForumPages.recoverpassword );
		}
	}
}
