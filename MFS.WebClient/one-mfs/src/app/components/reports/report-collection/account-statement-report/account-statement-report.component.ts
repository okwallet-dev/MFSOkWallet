import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';

@Component({
  selector: 'account-statement-report',
  templateUrl: './account-statement-report.component.html',
  styleUrls: ['./account-statement-report.component.css']
})
export class AccountStatementReportComponent implements OnInit {

    model: any;
    constructor(private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
    }

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
            obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            obj.mphone = this.model.mphone;  
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
        }        
    }

    validate(): any {
        if (!this.model.fromDate || !this.model.toDate || !this.model.mphone) {
            return false;
        }
        else {
            return true;
        }
    }

}
