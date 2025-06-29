import { Routes } from '@angular/router';
import { AuthGuard } from './auth/guards/auth.guard';
import { NotFoundComponent } from './shared/components/errors/not-found/not-found.component';

export const appRoutes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        loadChildren: () =>
        import('./home/home.module').then(m => m.HomeModule)
    },
    {
        path: 'auth',
        loadChildren: () =>
        import('./auth/auth.module').then(m => m.AuthModule)
    },
    {
        path: 'sklep',
        loadChildren: () =>
        import('./products/products.module').then(m => m.ProductsModule)
    },
    {
        path: 'koszyk',
        loadChildren: () =>
        import('./cart/cart.module').then(m => m.CartModule)
    },
    {
        path: 'newsletter',
        loadChildren: () =>
        import('./newsletter/newsletter.module').then(m => m.NewsletterModule)
    },
    {
        path: 'moje-konto',
        canActivate: [AuthGuard],
        loadChildren: () =>
        import('./account/account.module').then(m => m.AccountModule),
    },
    { path: '**', component: NotFoundComponent },
];