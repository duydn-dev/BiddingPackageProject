

import { NgModule } from "@angular/core";
import {ToastModule} from 'primeng/toast';
import {TableModule} from 'primeng/table';
import {PaginatorModule} from 'primeng/paginator';
import {DialogModule} from 'primeng/dialog';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {InputTextModule} from 'primeng/inputtext';
import {DropdownModule} from 'primeng/dropdown';
import {FileUploadModule} from 'primeng/fileupload';
import {TreeModule} from 'primeng/tree';
import {CalendarModule} from 'primeng/calendar';
import {InputTextareaModule} from 'primeng/inputtextarea';
import {CheckboxModule} from 'primeng/checkbox';
import {OrderListModule} from 'primeng/orderlist';
import {StepsModule} from 'primeng/steps';
import {RadioButtonModule} from 'primeng/radiobutton';
import {ProgressSpinnerModule} from 'primeng/progressspinner';

@NgModule({
    exports: [
        ToastModule,
        TableModule,
        PaginatorModule,
        DialogModule,
        ConfirmDialogModule,
        InputTextModule,
        DropdownModule,
        FileUploadModule,
        TreeModule,
        CalendarModule,
        InputTextareaModule,
        CheckboxModule,
        OrderListModule,
        StepsModule,
        RadioButtonModule,
        ProgressSpinnerModule
    ]
})
export class PrimeNgModule {}