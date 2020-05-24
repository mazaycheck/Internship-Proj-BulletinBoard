import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/services/Repositories/user.service';
import { UserForDetail } from 'src/app/Models/UserForDetail';
import { AdminService } from 'src/app/services/admin/admin.service';
import { PageEvent } from '@angular/material/paginator';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { UserRolesUpdateComponent } from './user-roles-update/user-roles-update.component';
import { CheckboxUpdateModalComponent } from '../checkbox-update-modal/checkbox-update-modal.component';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  allUsersFromDb: UserForDetail[] = [];
  allRolesFromDb: string[] = [];
  allUserRoles: string[];
  displayedColumns = ['username', 'email', 'registrationdate', 'roles', 'active', 'action'];
  temporaryRoles: string[] = [];


  // Page data
  queryOptions: { pageNumber: number, pageSize: number, query?: string } = { pageNumber: 1, pageSize: 10 };
  totalEntriesInDb: number;
  currentPageNumber: number;
  currentPageSize: number;
  pageSizeOptions = [10, 25, 50];

  // Search
  filter = new FormControl('');

  constructor(private toastr: ToastrService, private userService: UserService, private adminService: AdminService,
    private dialog: MatDialog) { }

  ngOnInit() {
    this.getAllUsers();
    this.getAllRoles();
    this.filter.valueChanges.pipe(debounceTime(500), distinctUntilChanged())
      .subscribe(x => { this.queryOptions.query = x; this.getAllUsers(); });
  }

  refresh(){
    this.getAllUsers();
    this.getAllRoles();    
  }

  getAllRoles() {
    this.adminService.getRoles().subscribe(response => {
      this.allRolesFromDb = response;

    });
  }

  accountActivation(user: UserForDetail) {
    if (user.isActive) {
      this.adminService.unlockAccount(user.userId).subscribe(response => {
        this.toastr.success(`Account ${user.email} unlocked`);
        this.refresh();
      });
    } else {
      this.adminService.deactivateAccount(user.userId).subscribe(reponse => {
        this.toastr.warning(`Account ${user.email} locked `);
        this.refresh();
      });
    }
  }

  getAllUsers() {
    this.userService.getAll(this.queryOptions).subscribe(response => {
      this.allUsersFromDb = response.pageData;
      this.totalEntriesInDb = response.totalEntries;
      this.currentPageNumber = response.pageNumber;
      this.currentPageSize = response.currentPageSize;
      this.allUsersEditOff();
    });
  }

  allUsersEditOff() {
    this.allUsersFromDb.forEach(element => {
      element.edit = false;
    });

  }



  editCancel(entity) {
    this.allUsersEditOff();
  }

  update(user) {
    this.onUpdateClicked(user);
  }

  pageClicked($event: PageEvent) {
    this.queryOptions.pageNumber = $event.pageIndex + 1;
    this.queryOptions.pageSize = $event.pageSize;
    this.getAllUsers();

  }

  onUpdateClicked(user: UserForDetail) {
    const dataToInject = {
      identity: user.email,
      title: user.userName,
      service: this.adminService,
      entity: user,
      allSelectOptions: this.allRolesFromDb,
      currentSelection: user.roles
    };
    const config = new MatDialogConfig();
    config.minWidth = '380px';
    config.width = '600px';
    config.data = dataToInject;
    this.dialog.open(CheckboxUpdateModalComponent, config);
    this.dialog.afterAllClosed.subscribe(x => {
      this.getAllUsers();
    });
  }


}
