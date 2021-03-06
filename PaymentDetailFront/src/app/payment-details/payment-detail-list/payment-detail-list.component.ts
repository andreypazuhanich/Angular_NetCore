import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PaymentDetail } from 'src/app/shared/payment-detail.model';
import { PaymentDetailService } from 'src/app/shared/payment-detail.service';

@Component({
  selector: 'app-payment-detail-list',
  templateUrl: './payment-detail-list.component.html',
  styles: [
  ]
})
export class PaymentDetailListComponent implements OnInit {

  constructor(public service: PaymentDetailService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.service.refreshList();
  }

  populateForm(pd: PaymentDetail){
    this.service.formData = Object.assign({},pd);
  }

  deletePaymentDetail(id: number){
    if(confirm("Вы уверены, что хотите удалить запись?"))
      this.service.deletePaymentDetail(id).subscribe(
        res =>{
          this.service.refreshList();
          this.toastr.warning("Запись успешно удалена");
        },
        err =>{
          console.log(err);
        }
      )
  }
}
