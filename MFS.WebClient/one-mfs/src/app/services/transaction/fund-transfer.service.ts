import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';
import { from } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FundTransferService {

    constructor(private http: HttpClient, private transactionService: MfsSettingService) { }

    getGlList() {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetGlList')
            .pipe(map(GlList => {
                return GlList;
            }));
    }
    getGlDetailsForRobi() {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/getGlDetailsForRobi')
            .pipe(map(GlList => {
                return GlList;
            }));
    }

    getGlDetailsForAirtel() {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/getGlDetailsForAirtel')
            .pipe(map(data => {
                return data;
            }));
    }

    getGlDetailsForBlink(): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/getGlDetailsForBlink')
            .pipe(map(GlList => {
                return GlList;
            }));
    }

    getACList(): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetACList')
            .pipe(map(ACList => {
                return ACList;
            }));
    }


    getAmountByGL(sysCode: string): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetAmountByGL?sysCode=' + sysCode)
            .pipe(map(amount => {
                return amount;
            }));
    }

    getAmountByAC(mPhone: string): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetAmountByAC?mPhone=' + mPhone)
            .pipe(map(amount => {
                return amount;
            }));
    }



    GetTransactionDetailsByPayAmount(fundTransferModel: any, from: string, to: string): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetTransactionDetailsByPayAmount?from=' + from + '&to=' + to, fundTransferModel)
            .pipe(map(transactionDetails => {
                return transactionDetails;
            }));
    }


    GetTransDtlForRobiByPayAmount(robiTopupStockEntryModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetTransDtlForRobiByPayAmount', robiTopupStockEntryModel)
            .pipe(map(transactionDetails => {
                return transactionDetails;
            }));
    }

    GetTransDtlForAirtelByPayAmount(airtelTopupStockEntryModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetTransDtlForAirtelByPayAmount', airtelTopupStockEntryModel)
            .pipe(map(transactionDetails => {
                return transactionDetails;
            }));
    }

    GetTransDtlForBlinkByPayAmount(blinkTopupStockEntryModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetTransDtlForBlinkByPayAmount', blinkTopupStockEntryModel)
            .pipe(map(transactionDetails => {
                return transactionDetails;
            }));
    }

    GetTransactionDetailsByTransactionNo(transNo: any): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetTransactionDetailsByTransactionNo?transNo=' + transNo)
            .pipe(map(transactionDetails => {
                return transactionDetails;
            }));
    }

    saveFundTransferEntry(fundTransferModel: any, from: string, to: string, isEditMode: boolean, event: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/saveFundTransferEntry?isEditMode=' + isEditMode + '&evnt=' + event + '&from=' + from + '&to=' + to, fundTransferModel)
            .pipe(map(model => {
                return model;
            }))
    }

    getTransactionList(hotkey: string, branchCode: string, transAmtLimt: number): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetTransactionList?hotkey=' + hotkey + '&branchCode=' + branchCode + '&transAmtLimt=' + transAmtLimt)
            .pipe(map(TransactionList => {
                return TransactionList;
            }));
    }

    AproveOrRejectFundTransfer(fundTransferModel: any, event: any, transType: string): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/AproveOrRejectFundTransfer?evnt=' + event + '&transType=' + transType, fundTransferModel)
            .pipe(map(model => {
                return model;
            }))
    }

    saveRobiTopupStockEntry(robiTopupStockEntryModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/saveRobiTopupStockEntry', robiTopupStockEntryModel)
            .pipe(map(model => {
                return model;
            }))
    }

    saveAirtelTopupStockEntry(airtelTopupStockEntryModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/saveAirtelTopupStockEntry', airtelTopupStockEntryModel)
            .pipe(map(model => {
                return model;
            }))
    }

    saveBlinkTopupStockEntry(blinkTopupStockEntryModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/saveBlinkTopupStockEntry', blinkTopupStockEntryModel)
            .pipe(map(model => {
                return model;
            }))
    }

    GetGLBalanceByGLSysCoaCode(fromSysCoaCode: any): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetGLBalanceByGLSysCoaCode?fromSysCoaCode=' + fromSysCoaCode)
            .pipe(map(amount => {
                return amount;
            }));
    }

    getCommissionGlListForDDL(): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetCommissionGlListForDDL')
            .pipe(map(ACList => {
                return ACList;
            }));
    }

    GetCommssionMobileList(sysCoaCode: any, entryOrApproval: string): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/GetCommssionMobileList?sysCoaCode=' + sysCoaCode + '&entryOrApproval=' + entryOrApproval)
            .pipe(map(data => {
                return data;
            }));
    }

    SaveCommissionEntry(toCatId: any, entryBy: any, entryBrCode: any, SelectedCommissionEntryModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/SaveCommissionEntry?toCatId=' + toCatId + '&entryBy=' + entryBy + '&entryBrCode=' + entryBrCode, SelectedCommissionEntryModel)
            .pipe(map(model => {
                return model;
            }))
    }

    checkPendingApproval(): any {
        return this.http.get<string>(this.transactionService.transactionApiServer + '/FundTransfer/CheckPendingApproval')
            .pipe(map(data => {
                return data;
            }));
    }

    AproveOrRejectCommissionEntry(event: any, entryBy: any, SelectedCommissionEntryModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/AproveOrRejectCommissionEntry?evnt=' + event + '&entryBy=' + entryBy, SelectedCommissionEntryModel)
            .pipe(map(model => {
                return model;
            }))
    }


}
