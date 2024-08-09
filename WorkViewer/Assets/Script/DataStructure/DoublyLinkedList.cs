public class DoublyLinkedListNode<T>
{
    public T Value;
    public DoublyLinkedListNode<T> Next;
    public DoublyLinkedListNode<T> Prev;

    public DoublyLinkedListNode(T value)
    {
        Value = value;
        Next = null;
        Prev = null;
    }
}

public class DoublyLinkedList<T>
{
    private DoublyLinkedListNode<T> head;
    public DoublyLinkedListNode<T> Head => head;
    private DoublyLinkedListNode<T> tail;
    public DoublyLinkedListNode<T> Tail => tail;


    public DoublyLinkedList()
    {
        head = null;
        tail = null;
    }

    public void AddFirst(T value)
    {
        var newNode = new DoublyLinkedListNode<T>(value);
        if (head == null)
        {
            head = tail = newNode;
        }
        else
        {
            newNode.Next = head;
            head.Prev = newNode;
            head = newNode;
        }
    }

    public void AddLast(T value)
    {
        var newNode = new DoublyLinkedListNode<T>(value);
        if (tail == null)
        {
            head = tail = newNode;
        }
        else
        {
            newNode.Prev = tail;
            tail.Next = newNode;
            tail = newNode;
        }
    }

    // Remove from the front
    public void RemoveFirst()
    {
        if (head != null)
        {
            head = head.Next;
            if (head != null)
                head.Prev = null;
            else
                tail = null;
        }
    }

    // Remove from the end
    public void RemoveLast()
    {
        if (tail != null)
        {
            tail = tail.Prev;
            if (tail != null)
                tail.Next = null;
            else
                head = null;
        }
    }

    // Find the node with a specific value
    public DoublyLinkedListNode<T> Find(T value)
    {
        var current = head;
        while (current != null)
        {
            if (current.Value.Equals(value))
                return current;
            current = current.Next;
        }
        return null;
    }
}
