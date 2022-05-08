export class ChatInList {
    id: number;
    name: string;
    isPrivate: boolean;
    lastMessage: LastMessage;

    constructor(id: number, name: string, isPrivate: boolean = false, lastMessage: LastMessage) {
        this.id = id;
        this.name = name;
        this.isPrivate = isPrivate;
        this.lastMessage = lastMessage;
    }
    
    get lastActivity() : string {
        const now = new Date();
        if (now.getDay() === this.lastMessage.sendDate.getDay()) {
            const hours = this.lastMessage.sendDate.getHours().toString().padStart(2, '0');
            const minutes = this.lastMessage.sendDate.getMinutes().toString().padStart(2, '0');

            return `${hours}:${minutes}`;
        } else {
            const date = this.lastMessage.sendDate.getDate().toString().padStart(2, '0');
            const month = (this.lastMessage.sendDate.getMonth()+1).toString().padStart(2, '0');

            return `${date}.${month}`;
        }
    }

    static deserialize(input: any): ChatInList {
        const name = input.name || input.members.map((m: any) => `${m.firstName} ${m.lastName}`).join(', ');
        
        return new ChatInList(
            input.id,
            name,
            input.isPrivate,
            new LastMessage(
                input.id,
                input.lastMessage.text,
                new Date(input.lastMessage.sendDate),
                input.lastMessage.isRemoved
            )
        );
    }
}

export class LastMessage {
    id: number;
    text: string;
    sendDate: Date;
    isRemoved: boolean;

    constructor(id: number, text: string, sendDate: Date, isRemoved: boolean = false){
        this.id = id;
        this.text = text;
        this.sendDate = sendDate;
        this.isRemoved = isRemoved;
    }
}