import { Request, Response, Application } from 'express'

export class TopicsInterface {

    public configureApp(topic: Topics, app: Application) {

        app.get('/topics', async function (req: Request, res: Response) {
            console.log('get /topics');

            res.json(await topic.getTopics());
        });

        app.post('/topics', async function (req: Request, res: Response) {
            console.log('post /topics');

            await topic.createTopic(
                req.body.name,
                parseInt(req.body.partitionCount),
            );

            res.sendStatus(200);
        });

        app.delete('/topics', async function (req: Request, res: Response) {
            console.log('delete /topics');

            await topic.deleteTopic(
                req.body.name,
            );

            res.sendStatus(200);
        });
    }
}