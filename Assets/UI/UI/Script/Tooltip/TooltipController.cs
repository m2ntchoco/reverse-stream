//using UnityEngine;
//using UnityEngine.UIElements;

//public class TooltipController
//{
//    private VisualElement tooltipRoot;
//    private Label tooltipName;
//    private Label gradeType;
//    private Label level;
//    private Label description;
//    private Label tooltipReqStat;
//    private Label tooltipAtk;
//    private Label price;

//    public void Initialize(VisualElement root, VisualTreeAsset tooltipUXML)
//    {
//        tooltipRoot = tooltipUXML.CloneTree();
//        tooltipRoot.style.display = DisplayStyle.None;
//        tooltipRoot.style.position = Position.Absolute;

//        tooltipName = tooltipRoot.Q<Label>("Name");
//        gradeType = tooltipRoot.Q<Label>("GradeType");
//        level = tooltipRoot.Q<Label>("Level");
//        description = tooltipRoot.Q<Label>("description");
//        tooltipReqStat = tooltipRoot.Q<Label>("reqStat");
//        tooltipAtk = tooltipRoot.Q<Label>("Atk");
//        price = tooltipRoot.Q<Label>("price");

//        root.Add(tooltipRoot);
//    }

//    public void Show(WeaponPrefabData weapon, Vector2 position)
//    {
//        if (weapon == null)
//        {
//            tooltipRoot.style.display = DisplayStyle.None;
//            return;
//        }

//        tooltipName.text = string.IsNullOrEmpty(weapon.weaponName)
//            ? $"{weapon.grade} {weapon.type}"
//            : weapon.weaponName;

//        gradeType.text = $"{weapon.grade}, {weapon.type}";
//        level.text = $"����: {weapon.level}";
//        description.text = weapon.description;

//        tooltipReqStat.text =
//            $"ü�� - {weapon.requiredHp} �� -  {weapon.requiredStr} ��ø - {weapon.requiredDex} ���� - {weapon.requiredInt} �� - {weapon.requiredluk}";


//        tooltipAtk.text =
//            $"���ݷ� : {weapon.attackPower}     " +
//            $"���ݼӵ� : {weapon.attackSpeed}\n\n" +
//            $"ġ��Ÿ Ȯ�� : {weapon.critChance}%   ġ��Ÿ ������ : {weapon.critDamage}%\n\n" +
//            $"��Ÿ� : {weapon.range}";

//        price.text = $"�ǸŰ�: {weapon.price} G";

//        //tooltipRoot.style.left = position.x + 10;
//        //tooltipRoot.style.top = position.y + 10;
//        tooltipRoot.style.display = DisplayStyle.Flex;

//        tooltipRoot.schedule.Execute(() =>
//        {
//            float width = tooltipRoot.resolvedStyle.width;
//            float height = tooltipRoot.resolvedStyle.height;

//            var rootBound = tooltipRoot.parent.worldBound;

//            float newX = position.x + 10;
//            float newY = position.y + 10;

//            if (newX + width > rootBound.xMax)
//            {
//                Debug.Log("[���� ����] ������ ��ħ �� �������� �̵�");
//                newX = position.x - width - 10;
//            }

//            if (newY + height > rootBound.yMax)
//            {
//                Debug.Log("[���� ����] �Ʒ��� ��ħ �� �������� �̵�");
//                newY = position.y - height - 10;
//            }

//            tooltipRoot.style.left = newX;
//            tooltipRoot.style.top = newY;
//        });

//    }

//    public void Hide()
//    {
//        tooltipRoot.style.display = DisplayStyle.None;
//    }
//}
