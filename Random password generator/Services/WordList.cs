namespace Random_password_generator.Services;

/// <summary>
/// A small offline word list used for passphrase generation. Not a cryptographic
/// artifact by itself — entropy comes from how PassphraseService samples it.
/// </summary>
static class WordList
{
    private static readonly string[] RawWords =
    {
        // Animals
        "ant", "bear", "bee", "bird", "bison", "boar", "camel", "cat", "cobra", "crab",
        "crane", "crow", "deer", "dog", "dolphin", "dove", "duck", "eagle", "eel", "elk",
        "falcon", "ferret", "finch", "fox", "frog", "gecko", "goat", "goose", "gopher", "grouse",
        "gull", "hare", "hawk", "heron", "horse", "hyena", "ibis", "jackal", "jaguar", "koala",
        "lark", "lemur", "leopard", "lion", "llama", "lynx", "magpie", "marlin", "mink", "mole",
        "moose", "moth", "mouse", "newt", "otter", "owl", "oyster", "panda", "panther", "parrot",
        "perch", "pigeon", "pony", "puma", "quail", "rabbit", "raccoon", "ram", "rat", "raven",
        "robin", "seal", "shark", "sheep", "shrew", "skunk", "sloth", "snail", "snake", "sparrow",
        "spider", "squid", "stork", "swan", "tiger", "toad", "trout", "turtle", "viper", "vulture",
        "walrus", "weasel", "whale", "wolf", "wombat", "wren", "zebra",

        // Colors
        "amber", "azure", "beige", "bronze", "coral", "crimson", "cyan", "emerald", "gold", "indigo",
        "ivory", "jade", "lavender", "lime", "magenta", "maroon", "navy", "olive", "onyx", "pearl",
        "rose", "ruby", "sapphire", "scarlet", "silver", "tan", "teal", "topaz", "turquoise", "violet",

        // Nature and weather
        "breeze", "brook", "canyon", "cave", "cliff", "cloud", "comet", "creek", "delta", "desert",
        "dune", "dusk", "dawn", "ember", "fern", "field", "flame", "flood", "fog", "forest",
        "frost", "glacier", "glen", "grove", "hail", "harbor", "haze", "hill", "island", "lagoon",
        "lake", "leaf", "marsh", "meadow", "mist", "moss", "mountain", "oasis", "ocean", "orbit",
        "peak", "pebble", "plain", "planet", "pond", "prairie", "rain", "ravine", "reef", "ridge",
        "river", "sand", "shore", "sky", "snow", "spark", "spring", "star", "stone", "storm",
        "stream", "summit", "swamp", "thunder", "tide", "trail", "tundra", "twilight", "valley", "volcano",
        "wave", "wind", "woodland",

        // Food
        "apple", "apricot", "bacon", "bagel", "banana", "basil", "berry", "biscuit", "bread", "broth",
        "butter", "cabbage", "candy", "carrot", "cheese", "cherry", "chili", "cocoa", "coconut", "coffee",
        "cookie", "cream", "cucumber", "curry", "fig", "garlic", "ginger", "grape", "honey", "kiwi",
        "lemon", "lentil", "lime", "mango", "maple", "melon", "mint", "muffin", "mushroom", "mustard",
        "noodle", "nutmeg", "oat", "olive", "onion", "pasta", "peach", "peanut", "pear", "pepper",
        "pickle", "pineapple", "plum", "popcorn", "potato", "pretzel", "pumpkin", "radish", "raisin", "salad",
        "sesame", "spinach", "syrup", "toast", "tomato", "vanilla", "waffle", "walnut", "wheat", "yogurt",

        // Objects and tools
        "anchor", "arrow", "axe", "badge", "banner", "barrel", "basket", "beacon", "bell", "blade",
        "blanket", "bolt", "boot", "bottle", "bowl", "brick", "bridge", "brush", "bucket", "buckle",
        "button", "cable", "camera", "candle", "canvas", "chain", "chair", "chest", "clock", "coin",
        "comb", "compass", "cord", "crate", "cup", "dagger", "desk", "dial", "disk", "dome",
        "drum", "engine", "fan", "flag", "flask", "fork", "frame", "gate", "gear", "glass",
        "globe", "glove", "hammer", "harp", "hat", "helmet", "hinge", "hook", "horn", "jacket",
        "jar", "key", "kite", "knife", "ladder", "lamp", "lantern", "lever", "lock", "loom",
        "mask", "mirror", "nail", "needle", "net", "oar", "paddle", "pan", "pendant", "pillar",
        "pipe", "plate", "pouch", "prism", "pulley", "quill", "rail", "ribbon", "ring", "rope",
        "rug", "sack", "saddle", "sail", "scale", "scarf", "screw", "shield", "shovel", "sled",
        "spoon", "stamp", "statue", "string", "switch", "sword", "table", "tent", "thread", "ticket",
        "tile", "torch", "tower", "tray", "tube", "umbrella", "urn", "vase", "vault", "vessel",
        "wagon", "wallet", "watch", "wheel", "whistle", "wire",

        // Adjectives
        "brave", "bright", "calm", "clever", "cool", "cozy", "crisp", "curious", "daring", "eager",
        "fair", "faithful", "fancy", "fierce", "fresh", "gentle", "giant", "glad", "grand", "happy",
        "honest", "humble", "jolly", "keen", "kind", "lively", "lucky", "mellow", "merry", "mighty",
        "noble", "plain", "proud", "quiet", "quick", "rapid", "rare", "rich", "royal", "rustic",
        "sharp", "shiny", "silent", "sleek", "smart", "smooth", "soft", "solid", "sturdy", "subtle",
        "swift", "tender", "tiny", "tough", "tranquil", "vast", "vivid", "warm", "wild", "wise",
        "witty", "young", "zesty",
    };

    public static readonly string[] Words = RawWords.Distinct().ToArray();
}
