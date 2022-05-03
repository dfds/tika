import { Request, Response, Application, NextFunction } from 'express'

export class ApiKeysInterface {
    public configureApp(apiKeys: ApiKeys, app: Application) {
    
        app.post('/api-keys', async function (req: Request, res: Response, next : NextFunction) {

            try {
                let apiKey = await apiKeys.createApiKey(
                    req.body.serviceAccountId,
                    req.body.description
                );
                res.json(apiKey);
    
            }
            catch (err) {
                next(err);
            }
        });

        app.get('/api-keys', async function (req: Request, res: Response, next : NextFunction) {

            try {
                res.json(await apiKeys.getApiKeys());
            }
            catch (err) {
                next(err);
            }
        });

        app.delete('/api-keys/:id', async function (req: Request, res: Response, next : NextFunction) {

            try {
                await apiKeys.deleteApiKey(req.params.id);

                res.sendStatus(200);    
            }
            catch (err) {
                next(err);
            }
        });
    }
}
