import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PrincipalComponent } from './presentation/pages/principal/principal.component';

const routes: Routes = [
  { path: '', component: PrincipalComponent },  // Ruta vacía como página principal
  { path: 'principal', component: PrincipalComponent },  // Ruta 'principal' sin la barra '/'
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TaskRoutingModule { }
