import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { BrandService } from 'src/app/services/Repositories/brand.service';
import { Brand } from 'src/app/Models/brand';
import { ToastrService } from 'ngx-toastr';
import { Category } from 'src/app/Models/Category';
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
  editSave(user) {
    // this.adminService.updateRoles(user.email, this.temporaryRoles).subscribe(response => {
    //   this.toastr.success(`${user.userName} updated`);
    //   user.roles = [...this.temporaryRoles];
    //   this.dialog.closeAll();
    // },
    //   error => {
    //     this.toastr.error(error);
    //   });
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
