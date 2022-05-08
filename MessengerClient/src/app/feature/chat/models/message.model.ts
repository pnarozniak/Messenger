import { User } from "src/app/shared/models/user.model";

export class Message {
    id?: number;
    text: string;
    sendDate: Date;
    isRemoved: boolean;
    sender: User;
    
    constructor(id: number | undefined, text: string, isRemoved: boolean, sendDate: Date, sender: User) {
        this.id = id;
        this.text = text;
        this.isRemoved = isRemoved;
        this.sendDate = sendDate;
        this.sender = sender;
    }

    static deserialize(input: any): Message {
        return new Message(
            input.id,
            input.text,
            input.isRemoved as boolean,
            new Date(input.sendDate),
            input.sender
        );
    }
}