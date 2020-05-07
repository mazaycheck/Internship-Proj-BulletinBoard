import { Component, OnInit, Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { BrandService } from 'src/app/services/Repositories/brand.service';
import { Brand } from 'src/app/Models/brand';
import { ToastrService } from 'ngx-toastr';
import { Category } from 'src/app/Models/Category';
import { AdminService } from 'src/app/services/admin/admin.service';
import { UserForDetail } from 'src/app/Models/UserForDetail';

@Component({
  selector: 'app-checkbox-update-modal',
  templateUrl: './checkbox-update-modal.component.html',
  styleUrls: ['./checkbox-update-modal.component.css']
})
export class CheckboxUpdateModalComponent implements OnInit {

  service: any;
  entity: any;
  temporarySelection: string[] = [];
  temporaryTitle: string;
  allSelectOptions: string[] = [];
  currentSelection: string[];
  identity: any;
  title: string;
  editTitle: boolean;

  constructor(@Inject(MAT_DIALOG_DATA) public injectedData, private toastr: ToastrService,
    private dialog: MatDialog) { }



  ngOnInit() {
    this.editTitle = this.injectedData.editTitle;
    this.identity = this.injectedData.identity;
    this.title = this.injectedData.title;
    this.temporaryTitle = this.injectedData.title;
    this.service = this.injectedData.service;
    this.entity = this.injectedData.entity;
    this.allSelectOptions = this.injectedData.allSelectOptions;
    this.currentSelection = this.injectedData.currentSelection;
    this.temporarySelection = [...this.injectedData.currentSelection];
  }
  editSave(entity) {
    this.service.update(this.identity, this.temporaryTitle, this.temporarySelection).subscribe(response => {
      this.toastr.success(`${this.title} updated`);
      this.dialog.closeAll();
    },
      error => {
        this.toastr.error(error);
      });
  }

  editCancel(entity) {
    this.dialog.closeAll();
  }

  toggleCheckBoxes() {
    if (!this.conditionMaxLength()) {
      this.temporarySelection = [...this.allSelectOptions];

    } else {
      this.temporarySelection = [];
    }
  }

  conditionMaxLength() {
    return this.allSelectOptions.length === this.temporarySelection.length;
  }

}
