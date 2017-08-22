import {Injectable} from '@angular/core';
import {AuthenticationTokenService} from '../../index';

@Injectable()

export class RoleService {
  constructor(private _authenticationService: AuthenticationTokenService) { }

  public isInRoles(roles: string[]): boolean {
    if (roles) {
      let userRole = this._authenticationService.user.role.toLowerCase();
      for (let i = 0; i < roles.length; i++) {
        if (userRole.indexOf(roles[i].toLowerCase()) !== -1) {
          return true;
        }
      }
    }
    return false;
  }
}
