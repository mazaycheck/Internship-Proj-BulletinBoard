import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from 'src/app/services/admin/admin.service';
import { UserForDetail } from 'src/app/Models/UserForDetail';

@Component({
  selector: 'app-user-roles-update',
  templateUrl: './user-roles-update.component.html',
  styleUrls: ['./user-roles-update.component.css']
})
export class UserRolesUpdateComponent implements OnInit {

  user: UserForDetail;
  temporaryRoles: string[] = [];
  allRoles: string[];
  toggled = false;

  constructor(@Inject(MAT_DIALOG_DATA) public injectedData, private adminService: AdminService, private toastr: ToastrService,
              private dialog: MatDialog) { }

  ngOnInit() {
    this.user = this.injectedData.user;
    this.allRoles = this.injectedData.roles;
    this.temporaryRoles = [...this.user.roles];
  }

  editCancel(entity) {
    this.dialog.closeAll();
  }

  toggleCheckBoxes() {
    if (!this.conditionMaxLength()) {
      this.temporaryRoles = [...this.allRoles];

    } else  {
      this.temporaryRoles = [];
    }
  }

  conditionMaxLength() {
    return this.allRoles.length === this.temporaryRoles.length;
  }
}
