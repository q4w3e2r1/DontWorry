using UnityEngine;

namespace SQL_Quest.Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;

        protected Rigidbody2D Rigidbody;
        protected Vector2 Direction;
        protected Animator Animator;

        private static readonly int IsWalking = Animator.StringToHash("is-walking");

        protected virtual void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        protected virtual void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed;
            Rigidbody.velocity = new Vector2(xVelocity, 0);

            Animator.SetBool(IsWalking, Direction.x != 0);

            UpdateSpriteDirection(Direction);
        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
                transform.localScale = new Vector3(multiplier, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-multiplier, 1, 1);
        }
    }
}